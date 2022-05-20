import { ProgrammingToolAPI } from "../../datasources/programmingToolAPI";

export default {
    programmingTools: async (_, { pageSize = 20, after }, { dataSources }: { dataSources: { programmingTool: ProgrammingToolAPI } }) => {
        return await dataSources.programmingTool.getAll();
    },
}