import { DataSource } from "apollo-datasource";
import { AgentBackend, Error } from "../domain";
import { error } from "../utils";
import lifecycle from "../lifecycle/agentBackend";
import _ from "lodash";
import { AgentBackendService } from "../services/agentBackend/agentBackendService";

export class AgentBackendAPI extends DataSource {
  private agentBackendService: AgentBackendService;

  constructor(agentBackendService: AgentBackendService) {
    super();
    this.agentBackendService = agentBackendService;
  }
  initialize({ config }: any) {
  }
  async addAgentBackend(agentBackend: AgentBackend): Promise<AgentBackend | Error> {
    const isValid = await this.agentBackendService.isValidForCreate(agentBackend)
    if (!isValid) {
      return error('invalid request');
    }

    await this.agentBackendService.prepareForCreate(agentBackend);
    await lifecycle.onBeforeCreated(agentBackend)

    const created = await this.agentBackendService.create(agentBackend);

    await lifecycle.onAfterCreated(created)
    return created;
  }
  async getAll(): Promise<AgentBackend[]> {
    return await this.agentBackendService.getAll();
  }

  async getById(id: string): Promise<AgentBackend | Error> {
    const a = await this.agentBackendService.getById(id);
    return a || error("not-found");
  }
}
