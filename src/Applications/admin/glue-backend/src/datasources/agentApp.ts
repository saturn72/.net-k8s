import _ from "lodash";
import { DataSource } from "apollo-datasource";
import lifecycle from "./../lifecycle/agentApp";
import { AgentApp, Error, FileUpload } from "../domain";
import { error, getFileExtension } from "../utils";
import { streamToBytes } from "./../services/media/mediaUtil";
import { AgentAppService } from '../services/agentApp/agentAppService'
import { MediaInfoService } from "../services/media/mediaInfoService";
import { getAgentAppFileName } from "../services/agentApp/agentAppUtil";

export class AgentAppAPI extends DataSource {
  private agentAppService: AgentAppService;
  private mediaInfoService: MediaInfoService;

  constructor(
    agentAppService: AgentAppService,
    mediaInfoService: MediaInfoService
  ) {
    super();
    this.agentAppService = agentAppService;
    this.mediaInfoService = mediaInfoService
  }
  initialize({ config }: any) {
  }
  async addAgentApp(agentApp: AgentApp, files: FileUpload[]): Promise<AgentApp | Error> {

    agentApp = await this.agentAppService.normalizeForCreate(agentApp);
    const isValid = await this.agentAppService.isValidForCreate(agentApp)
    if (!isValid) {
      return error('invalid request');
    }
    await this.agentAppService.prepareForCreate(agentApp);
    await lifecycle.onBeforeCreated(agentApp)
    if (files.length > 0) {
      const ext = files.length > 1 ? "zip" : getFileExtension(files[0].filename);
      agentApp.downloadableName = `${getAgentAppFileName(agentApp)}.${ext}`;
    }

    let created = await this.agentAppService.create(agentApp);
    if (files.length > 0) {
      created = await this.addFilesToAgentApp(created, files);
    }
    await lifecycle.onAfterCreated(created)
    return created;
  }
  private async addFilesToAgentApp(agentApp: AgentApp, files: FileUpload[]): Promise<AgentApp> {
    const mediaInfos = [];
    try {
      for (let i = 0; i < files.length; i++) {
        const f = files[i];
        const bytes = await streamToBytes(f.createReadStream());
        const d = {
          bytes,
          encoding: f.encoding,
          name: f.filename,
          mime: f.mimetype,
        }
        const mi = await this.mediaInfoService.create(d);
        mediaInfos.push(mi);
      }
      const updated = await this.agentAppService.patch(agentApp.id, { mediaInfoIds: mediaInfos.map(x => x.id) })
      if (updated) {
        updated.mediaInfos = mediaInfos;
      }
      return updated;
    }
    catch (err) {
      console.error(err);
    }
    return agentApp;
  }
  async getAll(): Promise<AgentApp[]> {
    return this.agentAppService.getAll();
  }

  async getById(id: string): Promise<AgentApp | Error> {
    const a = await this.agentAppService.getById(id);
    return a || error("not-found");
  }
}

