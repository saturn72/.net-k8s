import { AvosetGoAPI } from "../../datasources/avosetGoAPI";

export default {
    addAvosetGo: async (parent, { avosetGo }, { dataSources }: { dataSources: { avosetGo: AvosetGoAPI } }, info) => {
        return await dataSources.avosetGo.addAvosetGo(avosetGo);
    },
}