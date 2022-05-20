import { MessageBus } from '../domain';
export default {
    async onBeforeCreated(messageBus: MessageBus) { },
    async onAfterCreated(created: MessageBus) { },
    async onBeforeUpdated(before: MessageBus) { },
    async onAfterUpdated(before: MessageBus, after: MessageBus) { },
    async onBeforeDeleted(messageBus: MessageBus) { },
    async onAfterDeleted(messageBus: MessageBus) {
    },
}
