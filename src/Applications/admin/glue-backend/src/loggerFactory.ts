import consola, { Consola } from "consola";

let scopes = {};
const createLogger = (scope: string): Consola => {
    scopes[scope] = consola.withScope(scope);
    return scopes[scope];
}
export function getScopedLogger(scope: string): Consola {
    return scopes[scope] ?? createLogger(scope);
}