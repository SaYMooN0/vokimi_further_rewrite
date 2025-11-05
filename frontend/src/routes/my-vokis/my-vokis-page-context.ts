import { getContext, setContext } from "svelte";

export type MyVokiPageApi = {
    forceRefetch: () => void | Promise<void>;
    readonly isLoading: boolean;
};

const pageApiRegisterKey = Symbol('my-vokis-page-api-register');

export function setCurrentPage(registerFn: (api: MyVokiPageApi) => void) {
    setContext(pageApiRegisterKey, registerFn);
}

export function registerCurrentPageApi() {
    return getContext<(api: MyVokiPageApi) => void>(pageApiRegisterKey);
}