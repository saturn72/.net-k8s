import _ from "lodash";
import { AzureFileStorageProvider } from "./azureFileStorageProvider";
import { FileStorageProvider } from "./fileStorageProvider";

const azure = new AzureFileStorageProvider();

const providers = {
    azure
}
export default {
    connectAllProviders: async () => {
        await azure.connect({
            account: process.env.AZURE_STORAGE_ACCOUNT_NAME,
            accountKey: process.env.AZURE_STORAGE_ACCOUNT_KEY,
        });
    },
    get(key: string): FileStorageProvider {
        return providers[key] as FileStorageProvider;
    }
}