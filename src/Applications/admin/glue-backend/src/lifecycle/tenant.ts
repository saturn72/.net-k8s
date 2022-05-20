import { Tenant } from '../domain';
export default {
    async onBeforeCreated(tenant: Tenant) { },
    async onAfterCreated(created: Tenant) { },
    async onBeforeUpdated(before: Tenant) { },
    async onAfterUpdated(before: Tenant, after: Tenant) { },
    async onBeforeDeleted(tenant: Tenant) { },
    async onAfterDeleted(tenant: Tenant) {
    },
}
