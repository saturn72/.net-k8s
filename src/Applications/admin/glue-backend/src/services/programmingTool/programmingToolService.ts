import { PrismaClient } from "@prisma/client";
import { ProgrammingTool } from "../../domain";
import { isUri, isUrl } from "../urlUtil";

export interface ProgrammingToolService {
    create(programmingTool: ProgrammingTool): Promise<ProgrammingTool>;
    prepareForCreate(programmingTool: ProgrammingTool): Promise<void> | undefined;
    isValidForCreate(programmingTool: ProgrammingTool): Promise<boolean>;
    getAll(): Promise<ProgrammingTool[]>;

}

export class ProgrammingToolServiceImpl implements ProgrammingToolService {
    private prisma: PrismaClient;

    constructor(prisma: PrismaClient) {
        this.prisma = prisma;
    }
    async create(programmingTool: ProgrammingTool): Promise<ProgrammingTool> {
        const data: any = {
            ...programmingTool
        };
        const db = await this.prisma.programmingTool.create({ data });
        return db ? this.toDomainModel(db) : null;
    }

    prepareForCreate(programmingTool: ProgrammingTool): Promise<void> {
        return;
    }
    async isValidForCreate(programmingTool: ProgrammingTool): Promise<boolean> {
        const res = !!programmingTool.name &&
            !!programmingTool.url &&
            !!programmingTool.loginCallback;

        if (res &&
            isUrl(programmingTool.url) &&
            isUri(programmingTool.loginCallback)) {
            const names = await this.prisma.programmingTool.findMany({
                select: {
                    name: true
                }
            });
            return names.map(c => c.name).every(n => n !== programmingTool.name);
        }
        return res;
    }
    async getAll(): Promise<ProgrammingTool[]> {
        const allDbRecords = await this.prisma.programmingTool.findMany({
            select: {
                id: true,
                name: true,
                comment: true,
                url: true,
                avosetGoOnline: true
            }
        });
        return allDbRecords.map(this.toDomainModel);
    }
    private toDomainModel = (a: any): ProgrammingTool => {
        return {
            ...a
        };
    }
}