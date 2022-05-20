import { TenantAPI } from "../../datasources/tenant";

export default {
    tenants: async (_, { pageSize = 20, after }, { dataSources }: { dataSources: { tenants: TenantAPI } }) => {
        return await dataSources.tenants.getAll();
    },
    tenantById: async (_, { id }, { dataSources }: { dataSources: { tenants: TenantAPI } }) => {
        return await dataSources.tenants.getById(id);
    },
}