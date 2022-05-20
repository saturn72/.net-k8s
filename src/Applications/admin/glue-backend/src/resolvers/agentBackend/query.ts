import { AgentBackendAPI } from "../../datasources/agentBackend";

export default {
    agentBackends: async (_, { pageSize = 20, after }, { dataSources }: { dataSources: { agentBackends: AgentBackendAPI } }) => {
        return await dataSources.agentBackends.getAll();
    },
    // agentBackendById: async (_, { id }, { dataSources }: { dataSources: { agentBackends: AgentBackendAPI } }) => {
    //     return await dataSources.agentBackends.getById(id);
    // },
}