<script lang="ts">
	import DialogWithCloseButton from '$lib/components/dialogs/DialogWithCloseButton.svelte';
	import { StorageBucketMain } from '$lib/ts/backend-communication/storage-buckets';
	import type { Err } from '$lib/ts/err';
	import { toast } from 'svelte-sonner';
	import VokiCreationSaveAndCancelButtons from '../../../VokiCreationSaveAndCancelButtons.svelte';
	import CoverImageInput from './CoverImageInput.svelte';

	let { updateCoverToNew }: { updateCoverToNew: (newKey: string) => Promise<void> } = $props<{
		updateCoverToNew: (newKey: string) => Promise<void>;
	}>();
	let dialogState = $state<{ newKey: string; isInput: false } | { errs: Err[]; isInput: true }>({
		isInput: true,
		errs: []
	});
	let dialog = $state<DialogWithCloseButton>()!;
	export function open() {
		dialogState = { isInput: true, errs: [] };
		dialog.open();
	}
	function onDialogSaveButtonClick() {
		if (dialogState.isInput) {
			toast.error('An error has occurred. Please refresh the page');
		} else {
			updateCoverToNew(dialogState.newKey);
		}
		dialog.close();
	}
</script>

<DialogWithCloseButton bind:this={dialog} dialogId="voki-creation-cover-changing-dialog">
	{#if dialogState.isInput}
		<CoverImageInput
			onImageUploaded={(tempKey) => (dialogState = { isInput: false, newKey: tempKey })}
		/>
	{:else}
		<img src={StorageBucketMain.fileSrcWithVersion(dialogState.newKey)} alt="voki cover" />
		<VokiCreationSaveAndCancelButtons
			onCancel={() => dialog.close()}
			onSave={() => onDialogSaveButtonClick()}
		/>
	{/if}
</DialogWithCloseButton>

<style>
	:global(#voki-creation-cover-changing-dialog > .dialog-content) {
		width: 38rem;
		height: 26rem;
	}
	img {
		width: 100%;
		border-radius: 1rem;
		object-fit: fill;
		aspect-ratio: var(--voki-cover-aspect-ratio);
		animation: var(--default-fade-in-animation);
	}
</style>
