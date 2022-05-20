import { PrismaClient } from "@prisma/client";
import { Tenant, TenantState } from "../../domain";

export interface TenantService {
    create(tenant: Tenant): Promise<Tenant>;
    deleteById(id: string): Promise<Tenant>;
    prepareForCreate(tenant: Tenant): Promise<void> | undefined;
    isValidForCreate(tenant: Tenant): Promise<boolean>;
    getAll(): Promise<Tenant[]>;
    getById(id: string): Promise<Tenant>;
    updateById(id: string, data: any): Promise<any>;
}

export class TenantServiceImpl implements TenantService {
    private prisma: PrismaClient;

    constructor(prisma: PrismaClient) {
        this.prisma = prisma;
    }
    async create(tenant: Tenant): Promise<Tenant> {
        const data: any = {
            ...tenant
        };
        const db = await this.prisma.tenant.create({ data });
        return db ? this.toDomainModel(db) : null;
    }

    prepareForCreate(tenant: Tenant): Promise<void> {
        tenant.state = TenantState.inactive;
        return;
    }
    async isValidForCreate(tenant: Tenant): Promise<boolean> {
        const res = !!tenant.name && !!tenant.messageBusId;
        if (res) {
            const names = await this.prisma.tenant.findMany({
                select: {
                    name: true
                }
            });
            return names.map(c => c.name).every(n => n !== tenant.name);
        }
        return res;
    }
    async getAll(): Promise<Tenant[]> {
        const allDbRecords = await this.prisma.tenant.findMany();
        return allDbRecords.map(this.toDomainModel);
    }

    async getById(id: string): Promise<Tenant> {
        try {
            const a = await this.prisma.tenant.findUnique({
                where: {
                    id,
                },
                include: {
                    messageBus: true,
                    identityProviders: true
                }
            });
            return this.toDomainModel(a);
        } catch (error) {
            return undefined;
        }
    }
    async updateById(id: string, data: any): Promise<Tenant> {
        const db = await this.prisma.tenant.update({
            where: {
                id,
            },
            data
        });
        return this.toDomainModel(db);
    }
    async deleteById(id: string): Promise<Tenant> {
        const db = await this.prisma.tenant.delete({
            where: {
                id,
            },
        });
        return this.toDomainModel(db);
    }


    private toDomainModel = (a: any): Tenant => {
        return {
            ...a
        };
    }
}