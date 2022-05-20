import { AvosetGoAPI } from "../../datasources/avosetGoAPI";

export default {
    avosetGoOnlines: async (_, { pageSize = 20, after }, { dataSources }: { dataSources: { avosetGo: AvosetGoAPI } }) => {
        return await dataSources.avosetGo.getAll();
    },
}