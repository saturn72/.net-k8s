import { AgentApp } from "../../domain";

export interface AgentAppService {
    normalizeForCreate(agentApp: AgentApp): Promise<AgentApp>
    create(agentApp: AgentApp): Promise<AgentApp>
    prepareForCreate(agentApp: AgentApp): Promise<void> | undefined
    getById(id: string): Promise<AgentApp>
    getAll(): Promise<AgentApp[]>
    patch(id: string, data: any): Promise<AgentApp>
    isValidForCreate(agentApp: AgentApp): Promise<Boolean>
}