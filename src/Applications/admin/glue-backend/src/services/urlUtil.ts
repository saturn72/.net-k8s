const urlRegEx = new RegExp("(https:\/\/(?:www\.|(?!www))[a-zA-Z0-9][a-zA-Z0-9-]+[a-zA-Z0-9]\.[^\s]{2,}|www\.[a-zA-Z0-9][a-zA-Z0-9-]+[a-zA-Z0-9]\.[^\s]{2,}|https:\/\/(?:www\.|(?!www))[a-zA-Z0-9]+\.[^\s]{2,}|www\.[a-zA-Z0-9]+\.[^\s]{2,})");
const urlLocalhostRegEx = new RegExp("https?:\/\/(localhost):[0-9]{1,5}");

export function isUrl(value: string): boolean {
    let res: boolean = urlRegEx.test(value);
    if (!res && process.env.NODE_ENV == "development") {
        res = urlLocalhostRegEx.test(value);
    }
    return res;
}

export function isUri(value: string): boolean {
    return value.startsWith('/')
}