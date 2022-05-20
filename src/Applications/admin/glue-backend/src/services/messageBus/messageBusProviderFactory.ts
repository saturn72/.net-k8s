import MessageBusProvider from "./messageBusProvider";
import RabbitMqProvider from "./rabbitMqProvider";

const rabbitMq = new RabbitMqProvider();
const providers = [];
providers.push({ name: rabbitMq.providerName, provider: rabbitMq });

export default {
    getProvider: (providerName: string): MessageBusProvider => {
        return providers.find(p => p.name == providerName.toLowerCase())?.provider;
    },
}