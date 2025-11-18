<script lang="ts">
	import BaseContextMenu from '$lib/components/BaseContextMenu.svelte';
	import type { VokiAlbumPreviewData } from '../../types';

	let container: BaseContextMenu = $state<BaseContextMenu>()!;

	export function open(event: MouseEvent, album: VokiAlbumPreviewData) {
		currentAlbum = album;
		container.open(event.x, event.y);
	}
	let currentAlbum: VokiAlbumPreviewData | undefined = $state<VokiAlbumPreviewData>();
</script>

<BaseContextMenu bind:this={container} onAfterClose={() => (currentAlbum = undefined)}>
	{#if currentAlbum}
		<div>Edit</div>
		<div>Delete</div>
	{:else}
		<p>No album selected</p>
	{/if}
</BaseContextMenu>
