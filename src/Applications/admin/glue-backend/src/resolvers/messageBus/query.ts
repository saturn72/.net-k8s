import { MessageBusAPI } from "../../datasources/messageBus";

export default {
    messageBusConfig: async (_, { }, { dataSources }: { dataSources: { messageBus: MessageBusAPI } }) => {
        return {
            rabbitmq:
            {
                minUsernameLength: 10,
                minPasswordLength: 20
            }
        };
    },
    messageBuses: async (_, { pageSize = 20, after }, { dataSources }: { dataSources: { messageBus: MessageBusAPI } }) => {
        return await dataSources.messageBus.getAll();
    },
    messageBusById: async (_, { id }, { dataSources }: { dataSources: { messageBus: MessageBusAPI } }) => {
        return await dataSources.messageBus.getById(id);
    },
}