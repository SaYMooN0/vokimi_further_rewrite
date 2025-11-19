<script lang="ts">
	import DialogWithCloseButton from '$lib/components/dialogs/DialogWithCloseButton.svelte';
	import { ApiAlbums } from '$lib/ts/backend-communication/backend-services';
	import type { Err } from '$lib/ts/err';
	import type { VokiAlbumPreviewData } from '../../types';

	let album: VokiAlbumPreviewData | undefined = $state<VokiAlbumPreviewData>();
	let dialog = $state<DialogWithCloseButton>()!;

	async function confirmDeletion() {
		const response = await ApiAlbums.fetchJsonResponse;
	}
	let errs: Err[] = $state([]);
	export function open(newAlbum: VokiAlbumPreviewData) {
		errs = [];
		album = newAlbum;
		dialog.open();
	}
</script>

<DialogWithCloseButton bind:this={dialog} dialogId="confirm-album-deletion-dialog">
	{#if album}
		<h1>Are you sure you want to delete <span class="album-name">{album.name}</span> album?</h1>
		<label class="vokis-count">It has {album.vokisCount} Vokis</label>
		<button class="delete-album" onclick={() => confirmDeletion()}>Delete</button>
	{:else}
		<label>No album to delete selected</label>
	{/if}
</DialogWithCloseButton>
