import { TenantAPI } from "../../datasources/tenant";

export default {
    addTenant: async (parent, { tenant }, { dataSources }: { dataSources: { tenants: TenantAPI } }, info) => {
        return await dataSources.tenants.addTenant(tenant)
    },

    toggleTenantState: async (parent, { id }, { dataSources }: { dataSources: { tenants: TenantAPI } }, info) => {
        return await dataSources.tenants.toggleState(id)
    },
    deleteTenantById: async (parent, { id }, { dataSources }: { dataSources: { tenants: TenantAPI } }, info) => {
        return await dataSources.tenants.deleteById(id);
    }
}