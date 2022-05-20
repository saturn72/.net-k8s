import { AccountService } from "./accountService"
import { PrismaClient } from "@prisma/client";
import _ from "lodash";
import { Account, AccountState, Branch } from "../../domain"
import oidcProviderFactory from "../oidc/oidcProviderFactory"

export class AccountServiceImpl implements AccountService {
    private prisma: PrismaClient;
    private readonly include: any = {
        agentApps: {
            select: {
                accountId: true,
                agentApp: true,
                agentAppId: true,
                agentBackend: true,
                agentBackendId: true,
                platform: true,
            }
        },
        branches: true,
        oidcProvider: true,
        programmingTool: true
    };

    constructor(prisma: PrismaClient) {
        this.prisma = prisma;
    }
    async deleteById(id: string): Promise<Account> {
        const db = await this.prisma.account.delete({
            where: {
                id,
            },
        });
        return this.toDomainModel(db);
    }
    async getAll(): Promise<Account[]> {
        const allDbRecords = await this.prisma.account.findMany({ include: this.include });
        return allDbRecords.map(this.toDomainModel);
    }
    async getById(id: string): Promise<Account> {
        try {
            const a = await this.prisma.account.findUnique({
                where: {
                    id,
                },
                include: this.include,
            });
            return this.toDomainModel(a);
        } catch (error) {
            return undefined;
        }
    }
    async addAccount(account: Account): Promise<Account> {
        const data: any = {
            comment: account.comment,
            name: account.name,
            state: AccountState.draft,
        };

        data.agentApps = { create: account.agentApps }
        data.programmingTool = {
            connect: { id: account.programmingToolId }
        };
        data.oidcProvider = { create: account.oidcProvider };
        const db = await this.prisma.account.create({
            data,
            include: this.include,
        });
        return this.toDomainModel(db);
    }

    async updateById(id: string, data: any): Promise<any> {
        const db = await this.prisma.account.update({
            where: {
                id,
            },
            data,
            include: this.include,
        });
        return this.toDomainModel(db);
    }
    async isValidForCreate(account: Account): Promise<Boolean> {
        if (!account?.name || !account.oidcProvider) {
            return false;
        }
        const exist = await this.prisma.account.findMany({
            select: {
                name: true,
                oidcProvider: {
                    select: {
                        displayName: true
                    }
                }
            }
        });
        if (exist.find(n => n.name == account.name)) {
            return false;
        }

        const oidc = account.oidcProvider;

        if (!oidc.clientId ||
            !oidc.clientSecret ||
            !oidc.issuer ||
            !oidc.provider ||
            !oidc.scheme ||
            !oidc.accountUuid) {
            return false;
        }
        const provider = oidcProviderFactory.getOidcProvider(oidc.provider);
        return provider && provider.isValid(oidc);
    }

    async prepareForCreate(account: Account): Promise<void> {
        if (!account.oidcProvider.displayName) {
            account.oidcProvider.displayName = account.name
        }
        const names = await this.prisma.oidcProvider.findMany({
            select: {
                displayName: true
            },
            where: { accountId: account.id }
        });

        //unique display name
        let i = 1;
        let tmp = account.oidcProvider.displayName

        while (names.find(n => n.displayName == tmp)) {
            tmp = account.oidcProvider.displayName + `_${i}`;
            i++;
        }
        account.oidcProvider.displayName = tmp
    }

    async updateAccountBranches(account: Account): Promise<void> {
        const provider = oidcProviderFactory.getOidcProvider(account.oidcProvider.provider);
        if (!provider) {
            return;
        }
        const providerBranches = await provider.getAccountBranches(account);

        if (!providerBranches) {
            return;
        }
        const accountBranches = await this.prisma.branch.findMany({
            where: { accountId: account.id }
        });
        await this.deleteRedundantBranches(account.id, accountBranches, providerBranches);
        await this.addNewBranches(account.id, accountBranches, providerBranches);
    }

    private async addNewBranches(accountId: string, accountBranches: Branch[], providerBranches: Branch[]) {
        let create: any = accountBranches ?
            providerBranches.filter((x: Branch) =>
                (accountBranches?.findIndex((y: Branch) => y.branchId == x.branchId) < 0) ?? true
            ) :
            providerBranches;
        if (!create || _.isEmpty(create)) {
            return;
        }

        await this.prisma.account.update({
            where: {
                id: accountId
            },
            data: {
                branches: { create }
            }
        });
    }
    private async deleteRedundantBranches(accountId: string, accountBranches: Branch[], providerBranches: Branch[]) {
        const toDelete = accountBranches?.filter((x: Branch) =>
            providerBranches.findIndex((y: Branch) => y.branchId == x.branchId) < 0
        );
        if (!toDelete || _.isEmpty(toDelete)) {
            return;
        }

        const toDeleteIds = toDelete.map((x: Branch) => x.id);
        await this.prisma.account.update({
            where: { id: accountId },
            data: {
                branches: {
                    deleteMany: toDeleteIds.map((c: string) => { return { id: c } }),
                },
            }
        })
    }
    private toDomainModel(a: any): Account {
        return {
            ...a
        };
    }
}