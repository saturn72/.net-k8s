import { AgentAppService } from "../services/agentApp/agentAppService";
import { Request, Response } from 'express';
import { MediaInfoService } from "../services/media/mediaInfoService";
import _ from "lodash";
import { zipLocalFiles } from "../services/media/mediaUtil";
import { readdir } from "fs/promises";
import path from "path";

export class AgentAppController {
    agentAppService: AgentAppService;
    mediaInfoService: MediaInfoService;
    req: Request;
    res: Response;

    constructor(
        {
            req,
            res,
            agentAppService,
            mediaInfoService
        }) {
        this.req = req;
        this.res = res;
        this.agentAppService = agentAppService;
        this.mediaInfoService = mediaInfoService
    }

    async download() {
        const id = this.req.params.agentId
        const a = await this.agentAppService.getById(id);
        if (!a || !a.mediaInfoIds || a.mediaInfoIds.length == 0) {
            this.res.sendStatus(400);
            return;
        }
        const mis = await this.mediaInfoService.getBy({
            where: {
                id: { in: a.mediaInfoIds },
            }, select: {
                id: true,
                provider: true,
                providerPath: true,
                providerSpace: true,
            }
        });
        const location = await this.mediaInfoService.download(mis);
        const stat = await readdir(location)
        if (stat.length == 0) {
            this.res.sendStatus(400);
            return;
        }
        if (stat.length == 1) {
            this.res.download(path.join(location, stat[0]), a.downloadableName);
            return;
        }
        const dst = path.join(location, a.downloadableName)
        await zipLocalFiles(location, dst)
        this.res.download(dst, a.downloadableName);
    }
}