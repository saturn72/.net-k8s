import { MessageBusAPI } from "../../datasources/messageBus";

export default {
    addMessageBus: async (parent, { messageBus }, { dataSources }: { dataSources: { messageBus: MessageBusAPI } }, info) => {
        return await dataSources.messageBus.addMessageBus(messageBus)
    },
    deleteMessageBusById: async (parent, { id }, { dataSources }: { dataSources: { messageBus: MessageBusAPI } }, info) => {
        return await dataSources.messageBus.deleteById(id);
    }
}