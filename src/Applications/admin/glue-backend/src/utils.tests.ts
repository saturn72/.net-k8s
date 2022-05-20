import { tryParseJson } from "./utils";

describe("utils.tryParseJson tests", () => {
    it.each([
        "invalid",
        "{'k1':'v1', 'k2'}",
    ])("try parse json fails. value tested: %s", (s) => {
        expect(tryParseJson(s)).toBeFalsy();
    })
})