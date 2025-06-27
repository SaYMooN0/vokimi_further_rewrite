<script lang="ts">
	import LoginState from './c_sign_in_dialog/LoginState.svelte';
	import RegisterState from './c_sign_in_dialog/RegisterState.svelte';
	import type { SignInDialogState } from '../sign-in-dialog';
	import DialogWithCloseButton from '$lib/components/dialogs/DialogWithCloseButton.svelte';

	export function open(state: SignInDialogState | null = null) {
		dialog.open();
		if (state) {
			dialogState = state;
		}
		console.log(dialogState);
	}
	let dialogState = $state<SignInDialogState>('login');
	let dialog = $state<DialogWithCloseButton>()!;
	let email = $state('');
	let password = $state('');
</script>

<DialogWithCloseButton bind:this={dialog}>
	{#if dialogState === 'login'}
		<LoginState bind:email bind:password changeState={(val) => (dialogState = val)} />
	{:else if dialogState === 'register'}
		<RegisterState bind:email bind:password changeState={(val) => (dialogState = val)} />
	{:else}
		<RegisterState bind:email bind:password changeState={(val) => (dialogState = val)} />
	{/if}
</DialogWithCloseButton>

<style>
	:global(.dialog-content) {
		height: 28rem;
		width: 26rem;
	}
</style>