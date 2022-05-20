import OidcProvider from "./oidcProvider";
import OktaOidcProvider from "./oktaOidcProvider";

const okta = new OktaOidcProvider();
const providers = [];
providers.push({ name: okta.providerName, provider: okta });

export default {
    getOidcProvider: (providerName: string): OidcProvider => {
        return providers.find(p => p.name == providerName)?.provider;
    },
}