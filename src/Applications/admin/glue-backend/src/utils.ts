import crypto from "crypto";
import { Error } from "./domain";

export function generateUUID(length: number = 16): string {
  return crypto.randomBytes(length).toString("hex");
}
export function error(message: string): Error {
  return { code: 'ERROR', message, token: generateUUID() }
}
export function tryParseJson(text: string): any {
  try {
    return JSON.parse(text);
  } catch (e) {
    return undefined;
  }
}
export function winRelativePathToUnixs(windowsPath: string) {
  let px = windowsPath.replace(/^\\\\\?\\/, "").replace(/\\/g, '\/').replace(/\/\/+/g, '\/');
  px = px.replace(':', "");

  if (px.startsWith('/')) {
    px = px.slice(1);
  }
  return px;
}
export function getFileExtension(file: string) {
  return file.slice(file.lastIndexOf('.') + 1)
}