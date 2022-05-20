import MessageBusProvider from "./messageBusProvider";
import { MessageBus, MessageBusType } from "../../domain";
import { tryParseJson } from "../../utils";
import validUrl from "valid-url";

export default class RabbitMqProvider implements MessageBusProvider {
    readonly providerName: string = MessageBusType.rabbitmq;
    readonly icon: string = "mdi-rabbit";

    async isValid(messageBus: MessageBus, entries: { name: string, options: string }[]): Promise<boolean> {
        const options = tryParseJson(messageBus.options);
        const c = options && options.host && validUrl.isUri(options.host);
        if (!c) {
            return false;
        }
        const jo = [];
        entries.map(r => {
            const j = tryParseJson(r.options);
            if (j) {
                jo.push(j);
            }
        })

        return jo.every(n => n.host != options.host);
    }
}