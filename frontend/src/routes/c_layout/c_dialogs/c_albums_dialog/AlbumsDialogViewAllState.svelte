<script lang="ts">
	import { albumsStore, type VokisAlbumData } from '$lib/ts/stores/user-albums-store.svelte';

	interface Props {
		changeStateToCreateNew: () => void;
		albums: VokisAlbumData[];
	}
	let { changeStateToCreateNew, albums }: Props = $props();
</script>

<div class="all-albums-view">
	<div class="list">
		{#each albums as album}
			<div class="album">{album.name}</div>
		{/each}
	</div>
	<div class="create-new-btn" onclick={changeStateToCreateNew}>Create new albums button</div>
</div>

<style>
	.all-albums-view {
		height: 100%;
		width: 100%;
		display: grid;
		grid-template-columns: 1fr auto;
	}
	.list {
		display: flex;
		flex-direction: column;
	}
	.create-new-btn {
		padding: 0.25rem 1rem;
		border: none;
	}
</style>
