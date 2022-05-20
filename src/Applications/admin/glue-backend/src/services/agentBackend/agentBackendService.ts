import { PrismaClient } from "@prisma/client";
import { AgentBackend } from "../../domain"
import validUrl = require('valid-url');

export interface AgentBackendService {
    create(agentBackend: AgentBackend): Promise<AgentBackend>;
    isValidForCreate(agentBackend: AgentBackend): Promise<Boolean>;
    prepareForCreate(agentBackend: AgentBackend): Promise<void>;
    getAll(): Promise<AgentBackend[]>;
    getById(id: string): Promise<AgentBackend>;
}

export class AgentBackendServiceImpl implements AgentBackendServiceImpl {
    private prisma: PrismaClient;

    constructor(prisma: PrismaClient) {
        this.prisma = prisma;
    }

    async create(agentBackend: AgentBackend): Promise<AgentBackend> {
        const connectTenants = agentBackend.tenantIds.map((id) => { return { id } });

        const data: any = {
            alias: agentBackend.alias,
            comment: agentBackend.comment,
            discoverUri: agentBackend.discoverUri,
            healthcheckUrl: agentBackend.healthcheckUrl,
            url: agentBackend.url,
            websocketUri: agentBackend.websocketUri,
            tenants: {
                connect: connectTenants
            },
            identityProvider: {
                connect: [{
                    id: agentBackend.identityProviderId,
                }]
            }
        };
        const db = await this.prisma.identityProvider.create({
            include: {
                tenants: true
            },
            data
        });
        return db ? this.toDomainModel(db) : null;
    }
    async getAll(): Promise<AgentBackend[]> {
        const allDbRecords = await this.prisma.agentBackend.findMany(
            {
                include: {
                    tenants: true,
                    identityProvider: true,     
                }
            });
        return allDbRecords.map(this.toDomainModel);
    }


    public async isValidForCreate(agentBackend: AgentBackend): Promise<Boolean> {
        if (!agentBackend?.alias ||
            !agentBackend.url ||
            !validUrl.isHttpsUri(agentBackend.url) ||
            !validUrl.isHttpsUri(agentBackend.healthcheckUrl) ||
            !validUrl.isHttpsUri(agentBackend.url + agentBackend.websocketUri) ||
            !validUrl.isHttpsUri(agentBackend.url + agentBackend.discoverUri)
        ) {
            return false;
        }

        const exist = await this.prisma.agentBackend.findMany({
            select: {
                alias: true,
            }
        });

        const e = exist.find(a => a.alias == agentBackend.alias);
        if (e) { return false; }

        const idps = await this.prisma.identityProvider.findMany({
            select: {
                id: true,
            }
        });
        const eId = idps.find((x) => x.id == agentBackend.identityProviderId);
        if (!eId) { return false; }


        if (agentBackend.tenantIds.length > 0) {
            const tenants = await this.prisma.tenant.findMany({
                where: {
                    id: {
                        in: agentBackend.tenantIds
                    }
                },
                select: {
                    id: true
                }
            })
            if (tenants.length != agentBackend.tenantIds.length) {
                return false;
            }
        }
        return true;
    }
    async getById(id: string): Promise<AgentBackend> {
        try {
            const a = await this.prisma.agentBackend.findUnique({
                where: {
                    id,
                },
                include: {
                    tenants: true,
                    identityProvider: true,
                }
            });
            return this.toDomainModel(a);
        } catch (error) {
            return undefined;
        }
    }

    public async prepareForCreate(agentBackend: AgentBackend): Promise<void> {
        return undefined;
    }
    private toDomainModel(a: any): AgentBackend {
        return {
            id: a.id,
            alias: a.alias,
            comment: a.comment,
            discoverUri: a.discoverUri,
            identityProviderId: a.identityProviderId,
            identityProvider: a.identityProvider,
            healthcheckUrl: a.healthcheckUrl,
            url: a.url,
            websocketUri: a.websocketUri,
            tenants: a.tenants,
            tenantIds: a.tenantIds,
        }
    };
}