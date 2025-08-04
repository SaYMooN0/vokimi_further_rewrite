import { getContext, setContext } from 'svelte';

export type SignInDialogState = 'login' | 'signup' | 'confirmation-sent';


const signInKey = Symbol("open-sign-in-dialog-function");
type openSignInDialogFunction = (val: SignInDialogState) => void;

export function registerSignInDialogOpenFunction(openDialog: openSignInDialogFunction) {
	setContext(signInKey, openDialog);
}

export function getSignInDialogOpenFunction() {
	return getContext<openSignInDialogFunction>(signInKey);
}


