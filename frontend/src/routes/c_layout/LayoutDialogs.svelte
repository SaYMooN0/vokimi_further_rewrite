<script lang="ts">
	import ConfirmActionDialog from './c_dialogs/ConfirmActionDialog.svelte';
	import SignInDialog from './c_dialogs/SignInDialog.svelte';
	import {
		registerConfirmActionDialogOpenFunction,
		type ConfirmActionDialogContent
	} from './ts_layout_contexts/confirm-action-dialog-context';
	import {
		registerSignInDialogOpenFunction,
		type SignInDialogState
	} from './ts_layout_contexts/sign-in-dialog-context';

	let signInDialog = $state<SignInDialog>()!;
	let confirmActionDialog = $state<ConfirmActionDialog>()!;

	registerSignInDialogOpenFunction((state: SignInDialogState) => signInDialog.open(state));
	registerConfirmActionDialogOpenFunction({
		open: (content: ConfirmActionDialogContent) => confirmActionDialog.open(content),
		close: () => confirmActionDialog.close()
	});
</script>

<SignInDialog bind:this={signInDialog} />
<ConfirmActionDialog bind:this={confirmActionDialog} />
