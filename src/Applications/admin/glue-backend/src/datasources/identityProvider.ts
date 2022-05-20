import { DataSource } from "apollo-datasource";
import { IdentityProvider, Error } from "../domain";
import { error } from "../utils";
import { IdentityProviderService, IdentityProviderUpdateRequest } from "../services/identityProvider/identityProviderService";
import lifecycle from "../lifecycle/identityProvider";

export class IdentityProviderAPI extends DataSource {
  private idpService: IdentityProviderService;

  constructor(identityProviderService: IdentityProviderService) {
    super();
    this.idpService = identityProviderService;
  }

  async addIdentityProvider(identityProvider: IdentityProvider): Promise<IdentityProvider | Error> {
    const isValid = await this.idpService.isValidForCreate(identityProvider)
    if (!isValid) {
      return error('invalid request');
    }

    await this.idpService.prepareForCreate(identityProvider);
    await lifecycle.onBeforeCreated(identityProvider)
    const created = await this.idpService.create(identityProvider);

    await lifecycle.onAfterCreated(created)
    return created;
  }

  async update(identityProvider: IdentityProvider): Promise<IdentityProvider | Error> {
    const before = await this.idpService.getById(identityProvider.id)
    if (!before) {
      return error('invalid request');
    }
    const request: IdentityProviderUpdateRequest =
    {
      toUpdate: identityProvider,
      beforeUpdate: before
    };

    const isValid = await this.idpService.isValidForUpdate(request)
    if (!isValid) {
      return error('invalid request');
    }

    await this.idpService.prepareForUpdate(request);
    await lifecycle.onBeforeUpdated(request)
    const after = await this.idpService.update(request);

    await lifecycle.onAfterUpdated(request);
    return after;
  }

  async getAll(): Promise<IdentityProvider[]> {
    return await this.idpService.getAll();
  }

  async getById(id: string): Promise<IdentityProvider | Error> {
    const a = await this.idpService.getById(id);
    return a || error("not-found");
  }
}
