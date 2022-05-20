import { AvosetGoOnline } from '../domain';
export default {
    async onBeforeCreated(avosetGo: AvosetGoOnline) { },
    async onAfterCreated(created: AvosetGoOnline) { },
    async onBeforeUpdated(before: AvosetGoOnline) { },
    async onAfterUpdated(before: AvosetGoOnline, after: AvosetGoOnline) { },
    async onBeforeDeleted(avosetGo: AvosetGoOnline) { },
    async onAfterDeleted(avosetGo: AvosetGoOnline) {
    },
}
