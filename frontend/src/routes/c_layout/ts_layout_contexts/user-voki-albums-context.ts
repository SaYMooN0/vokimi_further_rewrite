import { setContext, getContext } from "svelte";

const openFunctionKey = Symbol("open-voki-albums-dialog-function");
type openVokiAlbumsDialogFunction = (vokiId: string) => void;

export function registerAlbumsDialogOpenFunction(openDialog: openVokiAlbumsDialogFunction) {
    setContext(openFunctionKey, openDialog);
}

export function getAlbumsDialogOpenFunction() {
    return getContext<openVokiAlbumsDialogFunction>(openFunctionKey);
}

