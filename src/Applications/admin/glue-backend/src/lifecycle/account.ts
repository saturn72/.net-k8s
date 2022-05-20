import { Account, AccountState, AgentAppConfig } from '../domain';
import rabbitMq from '../services/rabbitMq';
import { AccountServiceImpl } from "../services/account/accountServiceImpl"
import { PrismaClient } from '@prisma/client';
import { getScopedLogger } from './../loggerFactory';
import fileStorageProviderFactory from '../services/fileStorage/fileStorageProviderFactory';
import { getAccountAgentAppProviderPath as getAccountOrBranchAgentAppProviderPath, getAgentAppProviderPath } from '../services/agentApp/agentAppUtil';
import _ from 'lodash';
import path from 'path';
import { URL } from 'url';

const logger = getScopedLogger("account:lifecycle");

const configTopic = process.env.RABBITMQ_TOPIC;
if (!configTopic) {
    throw new Error("Missing env varialbe: \'RABBITMQ_TOPIC\'");
}

const accountUpdatedRoutingKey = process.env.RABBITMQ_ACCOUNT_UPDATED_ROUTING_KEY;
if (!accountUpdatedRoutingKey) {
    throw new Error("Missing env varialbe: \'RABBITMQ_ACCOUNT_UPDATED_ROUTING_KEY\'");
}
const accountDeletedRoutingKey = process.env.RABBITMQ_ACCOUNT_DELETED_ROUTING_KEY;
if (!accountDeletedRoutingKey) {
    throw new Error("Missing env varialbe: \'RABBITMQ_ACCOUNT_DELETED_ROUTING_KEY\'");
}
const prisma: PrismaClient = new PrismaClient();
const accountService = new AccountServiceImpl(prisma);

const uploadAgentsToFileStorageProvider = async (account: Account) => {
    let toUpload: {
        sourcePath: string,
        agentAppConfig: AgentAppConfig
    }[] =
        account.agentApps.map(aa => {
            return {
                sourcePath: getAgentAppProviderPath(aa.agentApp),
                agentAppConfig: aa
            };
        });
    toUpload = _.uniqBy(toUpload, 'sourcePath');
    const azure = fileStorageProviderFactory.get("azure");
    for (const tu of toUpload) {

        const json = buildAppSettingsForAccount(account, tu.agentAppConfig);
        const buffer = Buffer.from(JSON.stringify(json), 'utf-8');
        //build appsettings here
        try {
            //account
            let dstPath = getAccountOrBranchAgentAppProviderPath(tu.agentAppConfig.agentApp, account.oidcProvider.accountUuid, '')
            await azure.copy("glue", tu.sourcePath, "avoset", dstPath);
            await azure.uploadBuffer("glue", dstPath, buffer, "appsettings.account.json", false);
            dstPath = path.join(dstPath, "latest");
            await azure.copy("glue", tu.sourcePath, "avoset", dstPath);
            await azure.uploadBuffer("glue", dstPath, buffer, "appsettings.account.json", false);

            //branches
            for (const branch of account.branches) {
                dstPath = getAccountOrBranchAgentAppProviderPath(tu.agentAppConfig.agentApp, account.oidcProvider.accountUuid, branch.branchId)
                await azure.copy("glue", tu.sourcePath, "avoset", dstPath);
                await azure.uploadBuffer("glue", dstPath, buffer, "appsettings.account.json", false);

                dstPath = path.join(dstPath, "latest");
                await azure.copy("glue", tu.sourcePath, "avoset", dstPath);
                await azure.uploadBuffer("glue", dstPath, buffer, "appsettings.account.json", false);
            }
        }
        catch (err) {
            console.error(err);
        }

    }
}

const buildAppSettingsForAccount = async (account: Account, agentApp: AgentAppConfig) => {
    const loginCallback = new URL(account.programmingTool.loginCallback, account.programmingTool.url).href;
    console.error("this is loginCallback", loginCallback);

    return {
        discoveryDocument: {
            serverBaseAddress: agentApp.agentBackend.url,
            accountName: account.name,
            authScheme: account.oidcProvider.scheme,
            discoUri: agentApp.agentBackend.discoverUri,
            issuer: agentApp.agentBackend.identityProvider.url,
            loginCallback,
            webSocketUri: agentApp.agentBackend.websocketUri,
            healthCheckUri: agentApp.agentBackend.healthcheckUrl
        }
    }
}
export default {
    onBeforeCreated: async (account: Account) => {
    },
    async onAfterCreated(account: Account) {
        try {
            await accountService.updateAccountBranches(account);
        }
        catch (err) {
            logger.error("failed to update account branches", err)
        }
    },
    async onBeforeUpdated(before: Account) {
    },
    async onAfterUpdated(before: Account, after: Account) {
        await accountService.updateAccountBranches(after);
        if (before.state == after.state) {
            return;
        }
        const accounts = {
            set: [],
            remove: []
        };
        const key = after.state == AccountState.published ? "set" : "remove";
        accounts[key].push(after);

        await rabbitMq.sendToExchange(configTopic, accountUpdatedRoutingKey, { accounts });

        await uploadAgentsToFileStorageProvider(after);
    },
    async onBeforeDeleted(account: Account) { },
    async onAfterDeleted(account: Account) {
        const accounts = {
            remove: [account],
        };
        await rabbitMq.sendToExchange(configTopic, accountDeletedRoutingKey, { accounts });
    },
}
