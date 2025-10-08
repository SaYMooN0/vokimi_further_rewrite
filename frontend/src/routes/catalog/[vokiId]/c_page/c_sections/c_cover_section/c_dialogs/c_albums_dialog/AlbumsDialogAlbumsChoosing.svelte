<script lang="ts">
	import type { AlbumViewData } from '../add-voki-to-album-dialog-state';

	interface Props {
		albumsViewData: AlbumViewData[];
		isAlbumChosen: (albumId: string) => boolean;
		isAlbumChosenChanged: (albumId: string) => void;
		toggleAlbumChosen: (albumId: string) => void;
		changeToCreateNewAlbum: () => void;
	}
	let {
		albumsViewData,
		isAlbumChosenChanged,
		isAlbumChosen,
		toggleAlbumChosen,
		changeToCreateNewAlbum
	}: Props = $props();
</script>

<div class="all-albums-view">
	<div class="list">
		{#each albumsViewData as album}
			<div class="album">{album.name}</div>
		{/each}
		<button class="create-new-btn" onclick={() => changeToCreateNewAlbum()}>
			Create new albums button
		</button>
	</div>
</div>

<style>
	.all-albums-view {
		display: grid;
		width: 100%;
		height: 100%;
		grid-template-columns: 1fr auto;
	}

	.list {
		display: flex;
		flex-direction: column;
	}

	.create-new-btn {
		padding: 0.25rem 1rem;
		border: none;
		text-decoration: underline;
		background-color: var(--secondary);
		color: var(--secondary-foreground);
	}
</style>
