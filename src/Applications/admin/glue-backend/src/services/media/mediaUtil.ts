import getRawBody from 'raw-body';
import path from "path";
import fs, { WriteStream } from "fs";
import { readdir } from "fs/promises";
import { FileUpload } from "./../../domain";
import consts from "./../../consts";
import { finished } from 'stream/promises';
import archiver, { Archiver } from "archiver";
import { generateUUID } from "./../../utils";
import { Readable } from 'stream';

const getArchiver = (out: WriteStream): Archiver => {
    const archive = archiver('zip', { zlib: { level: 9 } })
    archive.on('warning', function (err) {
        if (err.code === 'ENOENT') {
            // log warning
        } else {
            // throw error
            throw err;
        }
    });
    archive.on('error', function (err) {
        throw err;
    });
    archive.pipe(out)

    return archive;
}
export async function streamToBytes(stream: Readable): Promise<Buffer> {
    const res = await getRawBody(stream);
    return (typeof res == 'string' || (res instanceof String)) ?
        Buffer.from(res, 'utf-8') :
        res;
}
export async function zipLocalFiles(src: string, dst: string): Promise<void> {
    const out = fs.createWriteStream(dst);
    const archive = getArchiver(out);

    const srcFiles = await readdir(src)
    for (const name of srcFiles) {
        const s = fs.createReadStream(path.join(src, name))
        archive.append(s, { name })
    }
    await archive.finalize();
    await finished(out);
}

export async function zipToLocal(files: FileUpload[]): Promise<{ location: string }> {
    const location = path.join(consts.mediaDirectory, consts.tempDirectory, `${generateUUID()}.zip`);
    const out = fs.createWriteStream(location);
    const archive = getArchiver(out);

    for (let i = 0; i < files.length; i++) {
        const f = files[i];
        const s = f.createReadStream()
        archive.append(s, { name: f.filename })
    }
    await archive.finalize();
    await finished(out);
    return { location };
}
export async function saveToLocal(file: FileUpload): Promise<{ location: string }> {
    const location = path.join(consts.mediaDirectory, consts.tempDirectory, file.filename);
    const out = fs.createWriteStream(location);

    const stream = file.createReadStream();
    stream.pipe(out);
    await finished(out);
    return { location }
}