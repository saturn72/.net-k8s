import { AccountAPI } from "../../datasources/account";

export default {
    addAccount: async (parent, { account, verify }, { dataSources }: { dataSources: { accounts: AccountAPI } }, info) => {
        return await dataSources.accounts.addAccount(account, verify)
    },
    tryVerifyAccount: async (parent, { id }, { dataSources }: { dataSources: { accounts: AccountAPI } }, info) => {
        return await dataSources.accounts.tryVerifyAccount(id)
    },
    togglePublish: async (parent, { id }, { dataSources }: { dataSources: { accounts: AccountAPI } }, info) => {
        return await dataSources.accounts.togglePublish(id)
    },
    deleteById: async (parent, { id }, { dataSources }: { dataSources: { accounts: AccountAPI } }, info) => {
        return await dataSources.accounts.deleteById(id);
    }
}