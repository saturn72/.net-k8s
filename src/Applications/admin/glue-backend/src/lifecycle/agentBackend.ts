import { AgentBackend } from "../domain";

export default {
    async onBeforeCreated(agentApp: AgentBackend) {
    },
    async onAfterCreated(agentApp: AgentBackend) {
    },
    async onBeforeUpdated(before: AgentBackend) {
    },
    async onAfterUpdated(before: AgentBackend, after: AgentBackend) {
    },
    async onBeforeDeleted(agentApp: AgentBackend) { },
    async onAfterDeleted(agentApp: AgentBackend) {
    },
}
