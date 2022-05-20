import { DataSource } from "apollo-datasource";
import { MessageBus, Error } from "../domain";
import { error } from "../utils";
import lifecycle from "../lifecycle/messageBus";
import { MessageBusService } from "../services/messageBus/messageBusService";

export class MessageBusAPI extends DataSource {
  private messageBusService: MessageBusService;

  constructor(messageBusService: MessageBusService) {
    super();
    this.messageBusService = messageBusService;
  }

  async addMessageBus(messageBus: MessageBus): Promise<MessageBus | Error> {
    const isValid = await this.messageBusService.isValidForCreate(messageBus)
    if (!isValid) {
      return error('invalid request');
    }

    await this.messageBusService.prepareForCreate(messageBus);
    await lifecycle.onBeforeCreated(messageBus)
    const created = await this.messageBusService.create(messageBus);

    await lifecycle.onAfterCreated(created)
    return created;
  }
  async getAll(): Promise<MessageBus[]> {
    return await this.messageBusService.getAll();
  }

  async getById(id: string): Promise<MessageBus | Error> {
    const a = await this.messageBusService.getById(id);
    return a || error("not-found");
  }

  async deleteById(id: string): Promise<MessageBus | Error> {
    const bus = await this.messageBusService.getById(id)
    if (!bus) {
      return error("not-found")
    }
    await lifecycle.onBeforeDeleted(bus);
    const del = await this.messageBusService.deleteById(id)
    await lifecycle.onAfterDeleted(del);
    return del;
  }
}
