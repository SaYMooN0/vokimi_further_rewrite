import { getContext, setContext } from 'svelte';

export type SignInDialogState = 'login' | 'signup' | 'confirmation-sent';


const key = Symbol("open-sign-in-dialog-function");
type openDialogFunction = (val: SignInDialogState) => void;

export function registerSignInDialogOpenFunction(openDialog: openDialogFunction) {
	setContext(key, openDialog);
}

export function getSignInDialogOpenFunction() {
	return getContext<openDialogFunction>(key);
}