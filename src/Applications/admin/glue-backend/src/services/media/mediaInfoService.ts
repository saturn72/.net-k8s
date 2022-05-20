import { MediaInfo } from "../../domain";

export interface MediaInfoService {
    create(mediaInfo: MediaInfo): Promise<MediaInfo>
    getBy(query: { where: any, select: any }): Promise<MediaInfo[]>;
    download(mediaInfos: MediaInfo[]): Promise<string>
}