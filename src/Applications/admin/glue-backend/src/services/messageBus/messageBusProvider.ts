import { MessageBus } from "../../domain";

export default interface MessageBusProvider {
    readonly providerName: string;
    readonly icon: string;

    isValid(messageBus: MessageBus, entries: { name: string, options: string }[]): Promise<boolean>;
}