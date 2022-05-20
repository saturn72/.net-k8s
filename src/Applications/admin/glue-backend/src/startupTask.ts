import consts from "./consts";
import path from "path";
import fs from "fs";
import fileStorageProviderFactory from "./services/fileStorage/fileStorageProviderFactory";
export default {
    onStartup: async () => {
        const mediaTemp = path.join(consts.mediaDirectory, consts.tempDirectory);
        fs.mkdirSync(mediaTemp, { recursive: true });

        await fileStorageProviderFactory.connectAllProviders();
    }
}