import { IdentityProviderAPI } from "../../datasources/identityProvider";

export default {
    addIdentityProvider: async (parent, { identityProvider }, { dataSources }: { dataSources: { identityProviders: IdentityProviderAPI } }, info) => {
        return await dataSources.identityProviders.addIdentityProvider(identityProvider)
    },
    updateIdentityProvider: async (parent, { identityProvider }, { dataSources }: { dataSources: { identityProviders: IdentityProviderAPI } }, info) => {
        return await dataSources.identityProviders.update(identityProvider)
    },
}