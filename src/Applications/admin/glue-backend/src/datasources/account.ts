import { DataSource } from "apollo-datasource";
import { Account, AccountState, Error } from "../domain";
import lifecycle from "./../lifecycle/account"
import { error } from "./../utils";
import oidcProviderFactory from "../services/oidc/oidcProviderFactory";
import { AccountService } from "../services/account/accountService";

export class AccountAPI extends DataSource {
  private accountService: AccountService;

  constructor({ accountService }: any) {
    super();
    this.accountService = accountService;
  }
  initialize({ config }: any) {
  }
  async addAccount(account: Account, verifyAfterAdd: Boolean): Promise<Account | Error> {
    const isValid = await this.accountService.isValidForCreate(account)
    if (!isValid) {
      return error('invalid request');
    }
    await this.accountService.prepareForCreate(account);

    await lifecycle.onBeforeCreated(account);
    let created = await this.accountService.addAccount(account);
    const provider = oidcProviderFactory.getOidcProvider(account.oidcProvider.provider)
    if (verifyAfterAdd) {
      const verified = await provider.verifyConfiguration(account.oidcProvider);
      created = await this.accountService.updateById(created.id, { verified });
    }
    await lifecycle.onAfterCreated(created)
    return created;
  }
  async tryVerifyAccount(accountId: string): Promise<string> {
    let account = await this.accountService.getById(accountId);
    if (!account) {
      return null;
    }
    const provider = oidcProviderFactory.getOidcProvider(account.oidcProvider.provider)
    if (!provider) {
      return null;
    }
    const verified = await provider.verifyConfiguration(account.oidcProvider);
    if (verified && account.state != AccountState.verified) {
      account = await this.accountService.updateById(account.id, { state: AccountState.verified });
    }
    return account.state;
  }

  async getAll(): Promise<Account[]> {
    return await this.accountService.getAll();
  }

  async getById(id: string): Promise<Account | Error> {
    const a = await this.accountService.getById(id);
    return a || error("not-found");
  }

  async togglePublish(id: string): Promise<AccountState> {
    const before = await this.accountService.getById(id)
    if (before.state == AccountState.draft) {
      return before.state;
    }

    await lifecycle.onBeforeUpdated(before);
    const newState = before.state == AccountState.published ? AccountState.verified : AccountState.published
    const after = await this.accountService.updateById(id, { state: newState });
    await lifecycle.onAfterUpdated(before, after);
    return after.state;
  }

  async deleteById(id: string): Promise<Account | Error> {
    const account = await this.accountService.getById(id)
    if (!account) {
      return error("not-found")
    }
    await lifecycle.onBeforeDeleted(account);
    const del = await this.accountService.deleteById(id)
    await lifecycle.onAfterDeleted(del);
    return del;
  }
}
