import { setContext, getContext } from "svelte";

const signInKey = Symbol("open-voki-albums-dialog-function");
type openVokiAlbumsDialogFunction = () => void;

export function registerSignInDialogOpenFunction(openDialog: openVokiAlbumsDialogFunction) {
    setContext(signInKey, openDialog);
}

export function getSignInDialogOpenFunction() {
    return getContext<openVokiAlbumsDialogFunction>(signInKey);
}

