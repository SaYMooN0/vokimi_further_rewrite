import { setContext, getContext } from "svelte";

const signInKey = Symbol("open-voki-albums-dialog-function");
type openVokiAlbumsDialogFunction = () => void;

export function registerAlbumsDialogOpenFunction(openDialog: openVokiAlbumsDialogFunction) {
    setContext(signInKey, openDialog);
}

export function getAlbumsDialogOpenFunction() {
    return getContext<openVokiAlbumsDialogFunction>(signInKey);
}

