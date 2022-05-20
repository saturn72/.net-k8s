import { PrismaClient } from "@prisma/client";
import { AgentApp } from "../domain";
import { getAgentAppProviderPath } from "../services/agentApp/agentAppUtil";
import fileStorageProviderFactory from "../services/fileStorage/fileStorageProviderFactory";
const prisma: PrismaClient = new PrismaClient();

export default {
    async onBeforeCreated(agentApp: AgentApp) {
    },
    async onAfterCreated(agentApp: AgentApp) {
        const azure = fileStorageProviderFactory.get("azure");
        const path = getAgentAppProviderPath(agentApp);
        const ids = [];
        agentApp.mediaInfos.forEach(mi => {
            azure.uploadBuffer("glue", path, mi.bytes, mi.name, false);
            ids.push(mi.id)
        });
        try {
            await prisma.mediaInfo.updateMany({
                where: {
                    id: {
                        in: ids,
                    },
                },
                data: {
                    provider: "azure",
                    providerPath: path,
                    providerSpace: 'glue'
                }
            })
        }
        catch (err) {
            console.error(err);
        }
    },
    async onBeforeUpdated(before: AgentApp) {
    },
    async onAfterUpdated(before: AgentApp, after: AgentApp) {
    },
    async onBeforeDeleted(agentApp: AgentApp) { },
    async onAfterDeleted(agentApp: AgentApp) {
    },
}

