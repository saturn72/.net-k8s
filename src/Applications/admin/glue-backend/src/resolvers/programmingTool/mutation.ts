import { ProgrammingToolAPI } from "../../datasources/programmingToolAPI";

export default {
    addProgrammingTool: async (parent, { programmingTool }, { dataSources }: { dataSources: { programmingTool: ProgrammingToolAPI } }, info) => {
        return await dataSources.programmingTool.addProgrammingTool(programmingTool);
    },
}