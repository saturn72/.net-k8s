import { AgentApp } from "../../domain";
import path from "path";

export function getAccountAgentAppProviderPath(agentApp: AgentApp, accountId: string, branchId: string): string {
    return path.join('agents', accountId, branchId, agentApp.type, agentApp.platform, agentApp.alias, agentApp.version)
}
export function getAgentAppProviderPath(agentApp: AgentApp): string {
    return path.join('agents', agentApp.type, agentApp.platform, agentApp.alias, agentApp.version)
}

export function getAgentAppFileName(agentApp: AgentApp): string {
    return `${agentApp.type}-agent-${agentApp.platform}-${agentApp.version}`
}
