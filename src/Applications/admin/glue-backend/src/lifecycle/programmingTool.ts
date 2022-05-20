import { ProgrammingTool } from '../domain';
export default {
    async onBeforeCreated(programmingTool: ProgrammingTool) { },
    async onAfterCreated(created: ProgrammingTool) { },
    async onBeforeUpdated(before: ProgrammingTool) { },
    async onAfterUpdated(before: ProgrammingTool, after: ProgrammingTool) { },
    async onBeforeDeleted(programmingTool: ProgrammingTool) { },
    async onAfterDeleted(programmingTool: ProgrammingTool) {
    },
}
