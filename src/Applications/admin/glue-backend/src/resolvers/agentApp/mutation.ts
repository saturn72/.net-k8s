import { AgentAppAPI } from "../../datasources/agentApp";
import { FileUpload } from "../../domain";

export default {
    addAgentApp: async (parent, { agentApp }, { dataSources }: { dataSources: { agentApps: AgentAppAPI } }, info) => {
        const files: FileUpload[] = agentApp.files ? await Promise.all(await agentApp.files) : [];
        return await dataSources.agentApps.addAgentApp(agentApp, files)
    },
}