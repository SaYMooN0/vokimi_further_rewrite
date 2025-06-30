<script lang="ts">
	import LoginState from './c_sign_in_dialog/LoginState.svelte';
	import type { SignInDialogState } from '../sign-in-dialog';
	import DialogWithCloseButton from '$lib/components/dialogs/DialogWithCloseButton.svelte';
	import ConfirmationLinkState from './c_sign_in_dialog/ConfirmationLinkState.svelte';
	import SignUpState from './c_sign_in_dialog/SignUpState.svelte';

	export function open(state: SignInDialogState | null = null) {
		dialog.open();
		if (state) {
			dialogState = state;
		}
	}
	let dialogState = $state<SignInDialogState>('login');
	let dialog = $state<DialogWithCloseButton>()!;
	let email = $state('');
	let password = $state('');
</script>

<DialogWithCloseButton bind:this={dialog} dialogId="sign-in-dialog">
	{#if dialogState === 'login'}
		<LoginState bind:email bind:password changeState={(val) => (dialogState = val)} />
	{:else if dialogState === 'signup'}
		<SignUpState bind:email bind:password changeState={(val) => (dialogState = val)} />
	{:else if dialogState === 'confirmation-sent'}
		<ConfirmationLinkState {email} />
	{:else}
		<SignUpState bind:email bind:password changeState={(val) => (dialogState = val)} />
	{/if}
</DialogWithCloseButton>

<style>
	:global(#sign-in-dialog) {
		display: flex;
		flex-direction: column;
		width: 28rem;
		height: 34rem;
	}
</style>
