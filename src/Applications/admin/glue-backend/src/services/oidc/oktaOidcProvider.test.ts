import { OidcProviderInfo } from "../../domain";
import OktaOidcProvider from "./oktaOidcProvider";

describe("Okta OIDC Provider tests", () => {
    it("name == okta", () => {
        const okta = new OktaOidcProvider();
        expect(okta.providerName).toBe("okta")
    });

    it.each([
        null as OidcProviderInfo,
        { rawInfo: 'fff' },
        { rawInfo: '{"ssws":"s"}' },
        { rawInfo: '{"ssws":"s", "domain":"d"}' },
    ])("isValid - return false on invalid data. Value tested = %o", async (o) => {
        const okta = new OktaOidcProvider();
        const v = await okta.isValid(o);
        expect(v).toBeFalsy()
    })
    it("isValid - return true", async () => {
        const o = { rawInfo: '{"ssws":"s", "domain":"d", "callback":"cb"}' };
        const okta = new OktaOidcProvider();
        const v = await okta.isValid(o);
        expect(v).toBeTruthy()
    })
})
