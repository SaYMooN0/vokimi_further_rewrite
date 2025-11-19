<script lang="ts">
	import DialogWithCloseButton from '$lib/components/dialogs/DialogWithCloseButton.svelte';
	import type { VokiAlbumPreviewData } from '../../types';

	let album: VokiAlbumPreviewData | undefined = $state(undefined)!;
	let dialog = $state<DialogWithCloseButton>()!;
	export function open(a: VokiAlbumPreviewData) {
		album = a;
		dialog.open();
	}
</script>

<DialogWithCloseButton
	bind:this={dialog}
	dialogId="edit-album-dialog"
	onBeforeClose={() => (album = undefined)}
>
	{#if album}
		<p>edit album {JSON.stringify(album)}</p>
	{:else}
		<p class="no-album-selcted">No album selected</p>
	{/if}
</DialogWithCloseButton>
