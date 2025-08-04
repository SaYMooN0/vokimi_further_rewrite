import { getContext, setContext, type Snippet } from "svelte";

export type ConfirmActionDialogButtons = {
	confirmBtnText: string;
	confirmBtnOnclick: () => void;
	cancelBtnText: string;
	cancelBtnOnclick: () => void;
};
export type ConfirmActionDialogMainContent = {
	subheading: string | undefined;
	mainText: string;
};
export type ConfirmActionDialogContent = {
	mainContent: ConfirmActionDialogMainContent | Snippet,
	dialogButtons: ConfirmActionDialogButtons | Snippet
};

const confirmActionKey = Symbol("open-confirm-action-dialog-function");
type openConfirmActionDialogFunction = (val: ConfirmActionDialogContent) => void;

export function registerConfirmActionDialogOpenFunction(openDialog: openConfirmActionDialogFunction) {
	setContext(confirmActionKey, openDialog);
}

export function getConfirmActionDialogOpenFunction() {
	return getContext<openConfirmActionDialogFunction>(confirmActionKey);
}