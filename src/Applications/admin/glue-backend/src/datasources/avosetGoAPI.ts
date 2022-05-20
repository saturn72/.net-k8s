import { DataSource } from "apollo-datasource";
import { AvosetGoOnline, Error } from "../domain";
import { AvosetGoService } from "../services/avosetGo/avosetGoService";
import { error } from "../utils";
import lifecycle from "./../lifecycle/avosetGo";

export class AvosetGoAPI extends DataSource {
    avosetGoService: AvosetGoService;

    constructor(
        avosetGoService: AvosetGoService,
    ) {
        super();
        this.avosetGoService = avosetGoService;
    }
    initialize({ config }: any) {
    }
    async getAll(): Promise<AvosetGoOnline[]> {
        return await this.avosetGoService.getAll();
    }
    async addAvosetGo(avosetGo: AvosetGoOnline): Promise<AvosetGoOnline | Error> {
        const isValid = await this.avosetGoService.isValidForCreate(avosetGo)
        if (!isValid) {
            return error('invalid request');
        }
        await this.avosetGoService.prepareForCreate(avosetGo);

        await lifecycle.onBeforeCreated(avosetGo);
        const created = await this.avosetGoService.create(avosetGo);

        if (created) {
            await lifecycle.onAfterCreated(created)
            return created;
        }
        return error("failed to create");

    }
}
