import { AccountAPI } from "../../datasources/account";

export default {
    accounts: async (_: any, { pageSize = 20, after }: any, { dataSources }: { dataSources: { accounts: AccountAPI } }) => {
        return await dataSources.accounts.getAll();
    },
    accountById: async (_, { id }, { dataSources }: { dataSources: { accounts: AccountAPI } }) => {
        return await dataSources.accounts.getById(id);
    },
}