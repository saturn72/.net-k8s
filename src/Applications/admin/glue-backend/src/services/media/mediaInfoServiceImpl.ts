import { PrismaClient } from "@prisma/client";
import _ from "lodash";
import path from "path";
import consts from "../../consts";
import { MediaInfo } from "../../domain";
import { generateUUID } from "../../utils";
import { fileStorageProviderSpaces } from "../fileStorage/fileStorageProvider";
import fileStorageProviderFactory from "../fileStorage/fileStorageProviderFactory";
import { MediaInfoService } from "./mediaInfoService";

export class MediaInfoServiceImpl implements MediaInfoService {

    private prisma: PrismaClient;

    constructor(prisma: PrismaClient) {
        this.prisma = prisma;
    }

    async create(mediaInfo: MediaInfo): Promise<MediaInfo> {
        return await this.prisma.mediaInfo.create({ data: mediaInfo });
    }

    async getBy(query: { where: any, select: any }): Promise<MediaInfo[]> {
        const res = await this.prisma.mediaInfo.findMany(query);
        return res as MediaInfo[];
    }
    async download(mediaInfos: MediaInfo[]): Promise<string> {
        const providers = mediaInfos.map(m => m.provider);
        const destination = path.join(consts.mediaDirectory, consts.tempDirectory, generateUUID())

        for (const p of providers) {
            const pItems = mediaInfos.filter(x => x.provider == p);
            const fsp = fileStorageProviderFactory.get(p);
            for (const s of fileStorageProviderSpaces) {
                const sItems = pItems.filter(x => x.providerSpace == s);
                const paths = _.uniq(sItems.map(x => x.providerPath));
                for (const p of paths) {
                    const l = await fsp.downloadFromLocation(s, p, destination);
                }
            }
        }
        return destination;
    }
}