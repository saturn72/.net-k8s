import {
    DirectoryCreateIfNotExistsResponse,
    ShareClient,
    ShareDirectoryClient,
    ShareServiceClient,
    StorageSharedKeyCredential
} from "@azure/storage-file-share";
import fs from "fs";
import _, { over } from "lodash";
import path from "path";
import { winRelativePathToUnixs as winRelativePathToPosix } from "../../utils";
import { FileStorageProvider } from "./fileStorageProvider";

export class AzureFileStorageProvider implements FileStorageProvider {
    private glue = "glue";
    private avoset = "avoset"
    private shareDirectoryClients: {
        glue: ShareDirectoryClient,
        avoset: ShareDirectoryClient
    } = {
            glue: undefined,
            avoset: undefined
        };

    async connect({
        account,
        accountKey
    }): Promise<void> {
        const credential = new StorageSharedKeyCredential(account, accountKey);
        const serviceClient = new ShareServiceClient(
            // When using AnonymousCredential, following url should include a valid SAS
            `https://${account}.file.core.windows.net`,
            credential
        );
        const glueShareClient = serviceClient.getShareClient(this.glue);
        const avosetShareClient = serviceClient.getShareClient(this.avoset);
        try {
            this.shareDirectoryClients.glue = await this.createDirectoryClient(glueShareClient, this.glue)
            this.shareDirectoryClients.avoset = await this.createDirectoryClient(avosetShareClient, this.avoset)
        }
        catch (err) {
            console.log(err);
        }
    }
    async copy(
        srcSpace: "glue" | "avoset" | string,
        srcPath: string,
        destSpace: "glue" | "avoset" | string,
        destPath: string,
        override: boolean = false): Promise<void> {

        if (!srcSpace || !destSpace || !srcPath || !destPath) {
            return;
        }
        const srcSpaceDc = (this.shareDirectoryClients[srcSpace] as ShareDirectoryClient);
        const destSpaceDc = (this.shareDirectoryClients[destSpace] as ShareDirectoryClient);
        if (!srcSpaceDc || !destSpaceDc) {
            return;
        }
        const src = winRelativePathToPosix(srcPath)
        const srcDc = srcSpaceDc.getDirectoryClient(src);

        const dest = winRelativePathToPosix(destPath)
        const destDc = await this.createSubDirectory(destSpaceDc, dest)
        if (!srcDc || !destDc) {
            return;
        }
        await this.copyDirectory(srcDc, destDc, override);
    }
    async copyDirectory(srcDc: ShareDirectoryClient, destDc: ShareDirectoryClient, override: boolean) {
        try {
            for await (const entity of srcDc.listFilesAndDirectories()) {
                if (entity.kind === "file") {
                    const srcFC = srcDc.getFileClient(entity.name);
                    if (!override && await srcFC.exists()) {
                        continue;
                    }
                    const buffer = await srcFC.downloadToBuffer();
                    const destFC = destDc.getFileClient(entity.name);
                    await destFC.create(buffer.byteLength);
                    await destFC.uploadRange(buffer, 0, buffer.byteLength);
                }
                if (entity.kind === "directory") {
                    const srcDcRec = srcDc.getDirectoryClient(entity.name)
                    const destDcRec = await this.createSubDirectory(destDc, entity.name)
                    await this.copyDirectory(srcDcRec, destDcRec, override);
                }
            }
        }
        catch (err) {
            console.error(err);
            return;
        }
    }
    async uploadBuffer(
        space: "glue" | "avoset",
        directory: string,
        buffer: Buffer,
        fileName: string,
        override: boolean): Promise<Boolean> {

        const sdc = (this.shareDirectoryClients[space] as ShareDirectoryClient);
        if (!sdc || !directory) {
            return false;
        }
        const dest = winRelativePathToPosix(directory);
        try {
            const dc = await this.createSubDirectory(sdc, dest)
            if (!dc) {
                return false;
            }
            const fc = dc.getFileClient(fileName);
            if (!override && await fc.exists()) {
                return false;
            }
            await fc.uploadData(buffer);
        }
        catch (err) {
            console.error(err);
            return false;
        }
        return true;
    }

    async uploadFiles(
        space: "glue" | "avoset",
        directory: string,
        filePaths: string[]): Promise<Boolean> {
        const dc = (this.shareDirectoryClients[space] as ShareDirectoryClient);
        if (!dc || !directory) {
            return false;
        }
        const dest = winRelativePathToPosix(directory)
        dc.getDirectoryClient(dest);
        dc.createIfNotExists();

        for (let i = 0; i < filePaths.length; i++) {
            const fp = filePaths[i];
            const up = winRelativePathToPosix(fp);
            const fpParts = _.without(up.split(path.posix.sep), null, undefined, "");
            const name = fpParts.at(-1);
            const fc = dc.getFileClient(name);
            try {
                await fc.uploadFile(fp);
            }
            catch (err) {
                console.error(err);
                return false;
            }
        }
        return true;
    }

    async downloadFromLocation(space: "glue" | "avoset" | string, location: string, destination: string): Promise<void> {
        const sdc = (this.shareDirectoryClients[space] as ShareDirectoryClient);
        if (!sdc || !location) {
            return;
        }
        const dest = winRelativePathToPosix(location)
        const dc = sdc.getDirectoryClient(dest);
        const names: string[] = [];
        try {
            for await (const entity of dc.listFilesAndDirectories()) {
                if (entity.kind === "file") {
                    names.push(entity.name)
                }
            }
            for (let i = 0; i < names.length; i++) {
                const cur = names[i];
                const fc = dc.getFileClient(cur);
                const unix = winRelativePathToPosix(destination)
                fs.mkdirSync(unix, { recursive: true })
                const tempFile = path.join(destination, cur);
                var dr = await fc.downloadToFile(tempFile);
            }
        }
        catch (err) {
            console.error(err);
            return;
        }
    }

    private async createSubDirectory(root: ShareDirectoryClient, sub: string): Promise<ShareDirectoryClient> {
        const parts = _.without(sub.split('/'), null, undefined, '');
        let dc = root;
        let dcr: DirectoryCreateIfNotExistsResponse;
        for await (const p of parts) {
            dc = dc.getDirectoryClient(p);
            try {
                dcr = await dc.createIfNotExists();

            }
            catch (err) {
                console.error(err);
                console.error(dcr.errorCode);
                console.error(dcr);
            }
        }
        return dc;
    }
    private async createDirectoryClient(client: ShareClient, directory: string): Promise<ShareDirectoryClient> {
        await client.createIfNotExists();
        const directoryClient = client.getDirectoryClient(directory);
        try {
            await directoryClient.createIfNotExists();
        }
        catch (err) {
            console.error(err);
        }
        return directoryClient;
    }
}