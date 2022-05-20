import { AgentAppAPI } from "../../datasources/agentApp";

export default {
    agentApps: async (_, { pageSize = 20, after }, { dataSources }: { dataSources: { agentApps: AgentAppAPI } }) => {
        return await dataSources.agentApps.getAll();
    },
    // agentAppById: async (_, { id }, { dataSources }: { dataSources: { agentApps: AgentAppAPI } }) => {
    //     return await dataSources.agentApps.getById(id);
    // },
}