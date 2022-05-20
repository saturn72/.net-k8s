// import { Account, OidcProviderInfo } from "../../domain";
// import accountService from "./accountService"
// import oidcProviderFactory from "./../oidc/oidcProviderFactory";
// import { JSDocEnumTag } from "typescript";

// // async/await can be used.
// it.each([
//     {} as any,
//     { name: "name" },
// ])('returns false on invalid account. account = %o', async (o) => {
//     const res = await accountService.isValidForCreate(o);
//     expect(res).toBeFalsy();
// });

// it.each([
//     { name: "name", oidc: {} },
//     { name: "name", oidc: { clientId: "c-id" } },
//     { name: "name", oidc: { clientId: "c-id", clientSecret: "c-s" } },
//     { name: "name", oidc: { clientId: "c-id", clientSecret: "c-s", issuer: "iss" } },
//     { name: "name", oidc: { clientId: "c-id", clientSecret: "c-s", issuer: "iss", provider: "p", } },])
//     ('returns false on invalid account oidc config. account = %o', async () => {
//         const res = await accountService.isValidForCreate(null);
//         expect(res).toBeFalsy();
//     });

// it('returns false on not exist provider', async () => {
//     jest.mock("./../oidc/oidcProviderFactory")
//     const mockOPF = oidcProviderFactory as jest.Mocked<typeof oidcProviderFactory>;
//     mockOPF.getOidcProvider.mockReturnValueOnce(null);

//     const acc: Account = { name: "name", oidc: { clientId: "c-id", clientSecret: "c-s", issuer: "iss", provider: "p", scheme: "sc" } };
//     const res = await accountService.isValidForCreate(acc);

//     expect(res).toBeFalsy();
//     expect(mockOPF.getOidcProvider).toHaveBeenCalled();
// });

// it('returns false on provider.invalid == false ', async () => {
//     jest.mock("./../oidc/oidcProviderFactory", () => {
//         return {
//             __esModule: true,
//             default: jest.fn(() => 'mocked getOidcProvider'),
//             getOidcProvider: { providerName: "p", isValid(info: OidcProviderInfo) { return true; } },
//         };
//     });
//     const acc: Account = { name: "name", oidc: { clientId: "c-id", clientSecret: "c-s", issuer: "iss", provider: "p", scheme: "sc" } };
//     const res = await accountService.isValidForCreate(acc);

//     expect(res).toBeFalsy();
// });
