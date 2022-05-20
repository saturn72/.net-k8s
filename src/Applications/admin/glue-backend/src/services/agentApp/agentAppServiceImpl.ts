import { PrismaClient } from "@prisma/client";
import { AgentApp } from "../../domain"
import { AgentAppService } from "./agentAppService";

export class AgentAppServiceImpl implements AgentAppService {
    private prisma: PrismaClient;

    constructor(prisma: PrismaClient) {
        this.prisma = prisma;
    }

    async create(agentApp: AgentApp): Promise<AgentApp> {
        const data: any = {
            ...agentApp
        };
        delete data.files;
        const db = await this.prisma.agentApp.create({ data });

        return db ? this.toDomainModel(db) : null;
    }

    async getById(id: string): Promise<AgentApp> {
        try {
            const a = await this.prisma.agentApp.findUnique({
                where: {
                    id,
                },
            });
            return this.toDomainModel(a);
        } catch (error) {
            return undefined;
        }
    }
    async getAll(): Promise<AgentApp[]> {
        const allDbRecords = await this.prisma.agentApp.findMany();
        return allDbRecords.map(this.toDomainModel);
    }
    async patch(id: string, data: any): Promise<AgentApp> {
        const db = await this.prisma.agentApp.update({
            where: {
                id,
            },
            data,
        });
        return db ? this.toDomainModel(db) : null;
    }
    async normalizeForCreate(agentApp: AgentApp): Promise<AgentApp> {
        const a = { ...agentApp };
        a.alias = a.alias.toLowerCase();

        while (a.alias.includes('  ') || a.alias.includes('\t')) {
            a.alias = a.alias.replace('  ', ' ').replace('\t', ' ')
        }
        a.alias.replace(' ', '-');
        return a;
    }
    async isValidForCreate(agentApp: AgentApp): Promise<Boolean> {
        if (!agentApp?.alias || !agentApp.platform || !agentApp.type || !agentApp.version) {
            return false;
        }

        const exist = await this.prisma.agentApp.findMany({
            select: {
                alias: true,
                platform: true,
                type: true,
                version: true,
            }
        });

        const e = exist.find(a =>
            a.alias == agentApp.alias &&
            a.version == agentApp.version &&
            a.platform == agentApp.platform);
        return !e;
    }

    async prepareForCreate(agentApp: AgentApp): Promise<void> | undefined {
        return undefined;
    }
    private toDomainModel = (a: any): AgentApp => {
        const res = {
            ...a
        };

        if (a.mediaInfoIds) {
            res.mediaInfoIds = a.mediaInfoIds?.map((m: string) => m)
        }
        return res;
    }
}