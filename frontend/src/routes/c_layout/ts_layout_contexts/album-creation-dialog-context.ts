import { setContext, getContext } from "svelte";



const openFunctionKey = Symbol("open-create-new-album-dialog-function");
type openCreateNewAlbumDialogFunction = () => void;

export function registerCreateNewAlbumOpenFunction(openDialog: openCreateNewAlbumDialogFunction) {
    setContext(openFunctionKey, openDialog);
}

export function getCreateNewAlbumOpenFunction() {
    return getContext<openCreateNewAlbumDialogFunction>(openFunctionKey);
}

