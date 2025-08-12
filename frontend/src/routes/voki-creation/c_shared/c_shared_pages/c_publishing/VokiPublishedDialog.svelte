<script lang="ts">
	import DialogWithCloseButton from '$lib/components/dialogs/DialogWithCloseButton.svelte';
	import { StorageBucketMain } from '$lib/ts/backend-communication/storage-buckets';
	import type { VokiSuccessfullyPublishedData } from '$lib/ts/backend-communication/voki-creation-backend-service';

	let dialog = $state<DialogWithCloseButton>()!;
	let vokiData = $state<VokiSuccessfullyPublishedData>();

	export function open(data: VokiSuccessfullyPublishedData) {
		vokiData = data;
		dialog.open();
	}
</script>

<DialogWithCloseButton bind:this={dialog} dialogId="voki-published-dialog">
	<img src={StorageBucketMain.fileSrc(vokiData!.cover)} />
	<p>
		Voki
		<span>{vokiData!.name}</span>
		has been successfully published
	</p>
</DialogWithCloseButton>

<style>
	:global(#sign-in-dialog .dialog-content) {
		width: 28rem;
		height: 34rem;
	}
</style>
