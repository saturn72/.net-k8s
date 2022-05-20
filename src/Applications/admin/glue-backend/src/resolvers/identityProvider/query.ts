import { IdentityProviderAPI } from "../../datasources/identityProvider";

export default {
    identityProviders: async (_, { pageSize = 20, after }, { dataSources }: { dataSources: { identityProviders: IdentityProviderAPI } }) => {
        return await dataSources.identityProviders.getAll();
    },
    identityProviderById: async (_, { id }, { dataSources }: { dataSources: { identityProviders: IdentityProviderAPI } }) => {
        return await dataSources.identityProviders.getById(id);
    },
}