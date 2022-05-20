import oidcProviderFactory from "./oidcProviderFactory";

describe("OIDC Provider Factory", () => {
    it("get not exists provider", () => {
        const p = oidcProviderFactory.getOidcProvider("not-exist")
        expect(p).toBeUndefined();
    }),
        it("get okta provider", () => {
            const p = oidcProviderFactory.getOidcProvider("okta")
            expect(p.providerName).toBe("okta");
        })
})