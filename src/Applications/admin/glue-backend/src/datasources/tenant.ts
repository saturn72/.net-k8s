import { DataSource } from "apollo-datasource";
import { Tenant, Error, TenantState } from "../domain";
import { error } from "../utils";
import lifecycle from "../lifecycle/tenant";
import { TenantService } from "../services/tenant/tenantService";

export class TenantAPI extends DataSource {
  private tenantService: TenantService;

  constructor(tenantService: TenantService) {
    super();
    this.tenantService = tenantService;
  }

  async addTenant(tenant: Tenant): Promise<Tenant | Error> {
    const isValid = await this.tenantService.isValidForCreate(tenant)
    if (!isValid) {
      return error('invalid request');
    }

    await this.tenantService.prepareForCreate(tenant);
    await lifecycle.onBeforeCreated(tenant)

    const created = await this.tenantService.create(tenant);

    await lifecycle.onAfterCreated(created)
    return created;
  }
  async getAll(): Promise<Tenant[]> {
    return await this.tenantService.getAll();
  }

  async getById(id: string): Promise<Tenant | Error> {
    const a = await this.tenantService.getById(id);
    return a || error("not-found");
  }

  async toggleState(id: any): Promise<Tenant | Error> {
    const before = await this.tenantService.getById(id)
    if (!before) {
      return error("not-found");
    }
    await lifecycle.onBeforeUpdated(before);

    const newState = before.state == TenantState.active ? TenantState.inactive : TenantState.active
    const after = await this.tenantService.updateById(id, { state: newState });

    if (!after) {
      return error("failed-to-toggle-state");
    }
    await lifecycle.onAfterUpdated(before, after);
    return after || error("unknown-failure");
  }

  async deleteById(id: string): Promise<Tenant | Error> {
    const bus = await this.tenantService.getById(id)
    if (!bus) {
      return error("not-found")
    }
    await lifecycle.onBeforeDeleted(bus);
    const del = await this.tenantService.deleteById(id)
    await lifecycle.onAfterDeleted(del);
    return del;
  }
}
