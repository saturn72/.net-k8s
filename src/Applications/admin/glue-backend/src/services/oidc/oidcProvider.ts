import { Account, Branch, OidcProviderInfo } from "../../domain";

export default interface OidcProvider {
    readonly providerName: string;
    isValid(info: OidcProviderInfo): Promise<Boolean>;
    getAccountBranches(account: Account): Promise<Array<Branch>>;
    verifyConfiguration(oidc: OidcProviderInfo): Promise<Boolean>;
}