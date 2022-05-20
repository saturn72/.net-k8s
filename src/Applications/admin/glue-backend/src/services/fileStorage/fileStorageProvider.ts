export const fileStorageProviderSpaces = ["avoset", "glue"];

export interface FileStorageProvider {
    copy(
        srcSpace: "glue" | "avoset" | string,
        srcPath: string,
        destSpace: "glue" | "avoset" | string,
        destPath: string): Promise<void>;

    downloadFromLocation(
        space: "glue" | "avoset" | string,
        location: string,
        destination: string): Promise<void>;

    uploadBuffer(
        space: "glue" | "avoset",
        directory: string,
        buffer: Buffer,
        fileName: string,
        override: boolean): Promise<Boolean>;

    uploadFiles(
        space: "glue" | "avoset",
        directory: string,
        filePaths: string[]): Promise<Boolean>;
}