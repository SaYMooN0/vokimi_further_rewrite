<script lang="ts">
	import LoginState from './c_sign_in_dialog/LoginState.svelte';
	import RegisterState from './c_sign_in_dialog/SignUpState.svelte';
	import type { SignInDialogState } from '../sign-in-dialog';
	import DialogWithCloseButton from '$lib/components/dialogs/DialogWithCloseButton.svelte';
	import ConfirmationLinkState from './c_sign_in_dialog/ConfirmationLinkState.svelte';

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
		<RegisterState bind:email bind:password changeState={(val) => (dialogState = val)} />
	{:else if dialogState === 'confirmation-sent'}
		<ConfirmationLinkState {email} />
	{:else}
		<RegisterState bind:email bind:password changeState={(val) => (dialogState = val)} />
	{/if}
</DialogWithCloseButton>

<style>
	:global(#sign-in-dialog) {
		width: 26rem;
		height: 34rem;
		display: flex;
		flex-direction: column;
	}
</style>
