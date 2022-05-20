import { IdentityProvider } from "../domain";
import { IdentityProviderUpdateRequest } from "../services/identityProvider/identityProviderService";

export default {
    async onBeforeCreated(idp: IdentityProvider) {
    },
    async onAfterCreated(idp: IdentityProvider) {
    },
    async onBeforeUpdated(request: IdentityProviderUpdateRequest) {
    },
    async onAfterUpdated(request: IdentityProviderUpdateRequest) {
    },
    async onBeforeDeleted(idp: IdentityProvider) { },
    async onAfterDeleted(idp: IdentityProvider) {
    },
}
