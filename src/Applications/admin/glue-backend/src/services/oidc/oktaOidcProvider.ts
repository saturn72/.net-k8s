import OidcProvider from "./oidcProvider";
import { tryParseJson } from "./../../utils"
import { Account, Branch, OidcProviderInfo } from "../../domain";
import axios, { Method as m } from "axios";
import { URL } from "url";
import { getScopedLogger } from "../../loggerFactory";

const logger = getScopedLogger('okta');

const GROUPS_URI = "/api/v1/groups";
const EVERYONE_GROUP_NAME = "Everyone";
const toBranch = ({ id, profile: { name, description } }): Branch => {
    return {
        description,
        name,
        branchId: id,
    };
}
const makeRequest = async (domain: string, ssws: string, method: "get" | "post" | "head", data: any = undefined): Promise<any> => {
    try {
        const res = await axios({
            method,
            url: new URL(GROUPS_URI, domain).href,
            headers: { 'Authorization': `SSWS ${ssws}` },
            data
        });
        return res.status === 200 ? res.data || true : undefined;
    }
    catch (err) {
        logger.error(err)
    }
    return undefined;

}
const getAllGroups = async (domain: string, ssws: string) => {
    return makeRequest(domain, ssws, "get")
};
const settingsEditorGroup = {
    name: "_settings_editor",
    description: "Group for pump settings editing to be used as MFA tag"
};
const addGroup = async ({ domain, ssws }, { name, description }) => {
    const data = {
        profile: { name, description }
    };
    const resData = await makeRequest(domain, ssws, "post", data);
    return resData ? toBranch(resData) : undefined;
};
export default class OktaOidcProvider implements OidcProvider {
    readonly providerName: string = "okta";

    async isValid(info: OidcProviderInfo): Promise<Boolean> {
        if (!info) {
            return false
        }
        const ri = tryParseJson(info.rawInfo);
        return ri && ri.ssws && ri.domain;
    }
    async verifyConfiguration(oidc: OidcProviderInfo): Promise<Boolean> {
        const ri = tryParseJson(oidc.rawInfo);
        return await makeRequest(ri.domain, ri.ssws, "head");
    }
    async getAccountBranches(account: Account): Promise<Branch[]> {
        const oidc = account.oidcProvider
        const ri = tryParseJson(oidc.rawInfo);
        const domain = ri.domain;
        const ssws = ri.ssws;
        if (!domain || !ssws) {
            return [];
        }
        let groups = await getAllGroups(domain, ssws);
        if (!groups) {
            return [];
        }
        if (!groups.find(x => x.profile.name == settingsEditorGroup.name)) {
            addGroup(ri, settingsEditorGroup);
            groups = await getAllGroups(domain, ssws);
        }
        return groups.filter(x => !x.profile.name.startsWith('_') && x.profile.name != EVERYONE_GROUP_NAME).map(g => toBranch(g))
    }
}