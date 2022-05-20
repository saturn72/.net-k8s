import { PrismaClient } from "@prisma/client";
import { IdentityProvider } from "../../domain"
import validUrl = require('valid-url');

export interface IdentityProviderUpdateRequest {
    toUpdate: IdentityProvider;
    beforeUpdate: IdentityProvider;
};

export interface IdentityProviderService {
    create(identityProvider: IdentityProvider): Promise<IdentityProvider>;
    isValidForCreate(identityProvider: IdentityProvider): Promise<Boolean>;
    prepareForCreate(identityProvider: IdentityProvider): Promise<void>;

    update(request: IdentityProviderUpdateRequest): Promise<IdentityProvider>;
    isValidForUpdate(request: IdentityProviderUpdateRequest): Promise<Boolean>;
    prepareForUpdate(request: IdentityProviderUpdateRequest): Promise<void>;

    getAll(): Promise<IdentityProvider[]>;
    getById(id: string): Promise<IdentityProvider>;
}


export class IdentityProviderServiceImpl implements IdentityProviderService {
    private prisma: PrismaClient;

    constructor(prisma: PrismaClient) {
        this.prisma = prisma;
    }
    async create(identityProvider: IdentityProvider): Promise<IdentityProvider> {
        const connect = identityProvider.tenantIds.map((id) => { return { id } });
        const data: any = {
            alias: identityProvider.alias,
            comment: identityProvider.comment,
            healthcheckUrl: identityProvider.healthcheckUrl,
            url: identityProvider.url,
            tenants: {
                connect
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

    async getAll(): Promise<IdentityProvider[]> {
        const allDbRecords = await this.prisma.identityProvider.findMany(
            {
                include: {
                    tenants: true
                }
            });
        return allDbRecords.map(this.toDomainModel);
    }

    async isValidForCreate(identityProvider: IdentityProvider): Promise<Boolean> {
        return await this.isValidForCreateOrUpdate(identityProvider);
    }

    async update(request: IdentityProviderUpdateRequest): Promise<IdentityProvider> {
        const idp = request.toUpdate;

        const connect = idp.tenantIds.map((id) => { return { id } });
        const filter = (e: string) => !idp.tenantIds.includes(e);
        const disconnect = request.beforeUpdate.tenantIds.filter(filter).map((id) => { return { id } });

        const data: any = {
            alias: idp.alias,
            comment: idp.comment,
            healthcheckUrl: idp.healthcheckUrl,
            url: idp.url,
            tenants: {
                connect,
                disconnect
            }
        };
        const db = await this.prisma.identityProvider.update({
            where: {
                id: idp.id
            },
            include: {
                tenants: true
            },
            data
        });
        return db ? this.toDomainModel(db) : null;
    }


    async isValidForUpdate(request: IdentityProviderUpdateRequest): Promise<Boolean> {
        return await this.isValidForCreateOrUpdate(request.toUpdate);
    }

    private async isValidForCreateOrUpdate(identityProvider: IdentityProvider): Promise<Boolean> {

        if (!identityProvider?.alias ||
            !identityProvider.url ||
            !validUrl.isHttpsUri(identityProvider.url) ||
            !identityProvider.healthcheckUrl ||
            !validUrl.isHttpsUri(identityProvider.healthcheckUrl)) {
            return false;
        }
        const exist = await this.prisma.identityProvider.findMany({
            select: {
                alias: true,
                id: true,
            }
        });
        const e = exist.find(a => a.alias == identityProvider.alias && a.id != identityProvider.id);
        if (e) {
            return false;
        }

        if (identityProvider.tenantIds.length > 0) {
            const tenants = await this.prisma.tenant.findMany({
                where: {
                    id: {
                        in: identityProvider.tenantIds
                    }
                }
            })
            if (tenants.length != identityProvider.tenantIds.length) {
                return false;
            }
        }
        return true;
    }

    async prepareForCreate(identityProvider: IdentityProvider): Promise<void> {
        return await this.prepareForCreateOrUpdate(identityProvider);
    }

    async prepareForUpdate(request: IdentityProviderUpdateRequest): Promise<void> {
        request.beforeUpdate.tenantIds = request.beforeUpdate.tenants.map(t => t.id);
        await this.prepareForCreateOrUpdate(request.toUpdate);
    }

    private async prepareForCreateOrUpdate(identityProvider: IdentityProvider): Promise<void> {
        identityProvider.alias = identityProvider.alias.trim();
        identityProvider.url = identityProvider.url.trim();
        identityProvider.healthcheckUrl = identityProvider.healthcheckUrl.trim();
        identityProvider.comment = identityProvider.comment?.trim();
    }


    async getById(id: string): Promise<IdentityProvider> {
        try {
            const a = await this.prisma.identityProvider.findUnique({
                where: {
                    id,
                },
                include: {
                    tenants: true
                }
            });
            return this.toDomainModel(a);
        } catch (error) {
            return undefined;
        }
    }
    private toDomainModel(a: any): IdentityProvider {
        return {
            id: a.id,
            alias: a.alias,
            comment: a.comment,
            url: a.url,
            healthcheckUrl: a.healthcheckUrl,
            tenantIds: a.tenantIds,
            tenants: a.tenants
        }
    };
}