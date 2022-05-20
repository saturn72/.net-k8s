import { DataSource } from "apollo-datasource";
import { ProgrammingTool, Error } from "../domain";
import { ProgrammingToolService } from "../services/programmingTool/programmingToolService";
import { error } from "../utils";
import lifecycle from "../lifecycle/programmingTool";

export class ProgrammingToolAPI extends DataSource {
    programmingToolService: ProgrammingToolService;

    constructor(
        programmingToolService: ProgrammingToolService,
    ) {
        super();
        this.programmingToolService = programmingToolService;
    }
    initialize({ config }: any) {
    }
    async getAll(): Promise<ProgrammingTool[]> {
        return await this.programmingToolService.getAll();
    }
    async addProgrammingTool(programmingTool: ProgrammingTool): Promise<ProgrammingTool | Error> {
        const isValid = await this.programmingToolService.isValidForCreate(programmingTool)
        if (!isValid) {
            return error('invalid request');
        }
        await this.programmingToolService.prepareForCreate(programmingTool);

        await lifecycle.onBeforeCreated(programmingTool);
        const created = await this.programmingToolService.create(programmingTool);

        if (created) {
            await lifecycle.onAfterCreated(created)
            return created;
        }
        return error("failed to create");

    }
}
