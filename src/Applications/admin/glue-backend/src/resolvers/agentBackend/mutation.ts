import { AgentBackendAPI } from "../../datasources/agentBackend";

export default {
    addAgentBackend: async (parent, { agentBackend }, { dataSources }: { dataSources: { agentBackends: AgentBackendAPI } }, info) => {
        return await dataSources.agentBackends.addAgentBackend(agentBackend)
    },
}