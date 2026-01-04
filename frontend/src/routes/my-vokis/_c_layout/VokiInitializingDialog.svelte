<script lang="ts">
	import DialogWithCloseButton from '$lib/components/dialogs/DialogWithCloseButton.svelte';
	import VokiInitializingDialogInputState from './_c_initializing_dialog/VokiInitializingDialogInputState.svelte';
	import VokiInitializingDialogInitializedState from './_c_initializing_dialog/VokiInitializingDialogInitializedState.svelte';
	import type { VokiType } from '$lib/ts/voki-type';

	let dialog = $state<DialogWithCloseButton>()!;
	export function open() {
		dialogState = { state: 'input' };
		dialog.open();
	}
	type DialogState =
		| { state: 'input' }
		| { state: 'initialized'; vokiId: string; vokiType: VokiType; vokiName: string };
	let dialogState: DialogState = $state({ state: 'input' });

	function onNewVokiInitialized(newVokiData: { id: string; name: string; type: VokiType }) {
		dialogState = {
			state: 'initialized',
			vokiId: newVokiData.id,
			vokiType: newVokiData.type,
			vokiName: newVokiData.name
		};
	}
</script>

<DialogWithCloseButton dialogId="voki-initializing-dialog" bind:this={dialog}>
	{#if dialogState.state === 'input'}
		<VokiInitializingDialogInputState onVokiInitializedSuccessfully={onNewVokiInitialized} />
	{:else if dialogState.state === 'initialized'}
		<VokiInitializingDialogInitializedState
			id={dialogState.vokiId}
			type={dialogState.vokiType}
			name={dialogState.vokiName}
		/>
	{/if}
</DialogWithCloseButton>

<style>
	:global(#voki-initializing-dialog .dialog-content) {
		width: 48rem;
		min-height: 38rem;
	}

	:global(#voki-initializing-dialog .dialog-content:has(.voki-initialized-container)) {
		width: unset;
		height: unset;
	}
</style>
