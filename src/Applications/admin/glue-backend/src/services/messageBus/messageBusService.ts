import { PrismaClient } from "@prisma/client";
import { MessageBus, MessageBusType } from "../../domain";
import messageBusProviderFactory from "./messageBusProviderFactory";

export interface MessageBusService {
    create(messageBus: MessageBus): Promise<MessageBus>;
    prepareForCreate(messageBus: MessageBus): Promise<void> | undefined;
    isValidForCreate(messageBus: MessageBus): Promise<boolean>;
    getAll(): Promise<MessageBus[]>;
    getById(id: string): Promise<MessageBus>;
    deleteById(id: string): Promise<MessageBus>;
    updateById(id: string, data: any): Promise<any>;
}

export class MessageBusServiceImpl implements MessageBusService {
    private prisma: PrismaClient;

    constructor(prisma: PrismaClient) {
        this.prisma = prisma;
    }
    async create(messageBus: MessageBus): Promise<MessageBus> {
        const data: any = {
            ...messageBus
        };
        const db = await this.prisma.messageBus.create({ data });
        return db ? this.toDomainModel(db) : null;
    }

    async prepareForCreate(messageBus: MessageBus): Promise<void> {
        messageBus.type = messageBus.type.toLowerCase();
        const provider = messageBusProviderFactory.getProvider(messageBus.type);
        messageBus.icon = provider.icon;
    }
    async isValidForCreate(messageBus: MessageBus): Promise<boolean> {
        const dbEntries = await this.prisma.messageBus.findMany({
            select: {
                name: true,
                options: true
            }
        });
        if (!messageBus.name || !dbEntries.every(n => n.name != messageBus.name)) {
            return false;
        }
        const entries = dbEntries.map(s => {
            return {
                name: s.name,
                options: s.options.toString()
            }
        });

        const provider = messageBusProviderFactory.getProvider(messageBus.type);
        if (!provider || !await provider.isValid(messageBus, entries)) {
            return false;
        }
        return true;
    }
    async getAll(): Promise<MessageBus[]> {
        const allDbRecords = await this.prisma.messageBus.findMany();
        return allDbRecords.map(this.toDomainModel);
    }

    async getById(id: string): Promise<MessageBus> {
        try {
            const a = await this.prisma.messageBus.findUnique({
                where: {
                    id,
                },
                include:
                {
                    tenants: true
                }
            });
            return this.toDomainModel(a);
        } catch (error) {
            return undefined;
        }
    }
    async updateById(id: string, data: any): Promise<MessageBus> {
        const db = await this.prisma.messageBus.update({
            where: {
                id,
            },
            data
        });
        return this.toDomainModel(db);
    }

    async deleteById(id: string): Promise<MessageBus> {
        const db = await this.prisma.messageBus.delete({
            where: {
                id,
            },
        });
        return this.toDomainModel(db);
    }
    private toDomainModel = (a: any): MessageBus => {
        return {
            ...a
        };
    }
}