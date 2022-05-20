import { PrismaClient } from "@prisma/client";
import { AvosetGoOnline } from "../../domain";

export interface AvosetGoService {
    create(avosetGo: AvosetGoOnline): Promise<AvosetGoOnline>;
    prepareForCreate(avosetGo: AvosetGoOnline): Promise<void> | undefined;
    isValidForCreate(avosetGo: AvosetGoOnline): Promise<boolean>;
    getAll(): Promise<AvosetGoOnline[]>;

}

export class AvosetGoServiceImpl implements AvosetGoService {
    private prisma: PrismaClient;

    constructor(prisma: PrismaClient) {
        this.prisma = prisma;
    }
    async create(avosetGo: AvosetGoOnline): Promise<AvosetGoOnline> {
        const data: any = {
            ...avosetGo
        };
        const db = await this.prisma.avosetGoOnline.create({ data });
        return db ? this.toDomainModel(db) : null;
    }

    prepareForCreate(avosetGo: AvosetGoOnline): Promise<void> {
        return;
    }
    async isValidForCreate(avosetGo: AvosetGoOnline): Promise<boolean> {
        const res = !!avosetGo.healthcheckUrl &&
            !!avosetGo.name &&
            !!avosetGo.url &&
            !!avosetGo.websocketUri;
        if (res) {
            const names = await this.prisma.avosetGoOnline.findMany({
                select: {
                    name: true
                }
            });
            return names.map(c => c.name).every(n => n !== avosetGo.name);
        }
        return res;
    }
    async getAll(): Promise<AvosetGoOnline[]> {
        const allDbRecords = await this.prisma.avosetGoOnline.findMany();
        return allDbRecords.map(this.toDomainModel);
    }
    private toDomainModel = (a: any): AvosetGoOnline => {
        return {
            ...a
        };
    }
}