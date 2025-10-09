<script lang="ts">
	import DefaultCheckBox from '$lib/components/inputs/DefaultCheckBox.svelte';
	import type { AlbumViewData } from '../add-voki-to-album-dialog-state.svelte';

	interface Props {
		albumsViewData: AlbumViewData[];
		albumIdToIsChosen: Record<string, boolean>;
		isAlbumChosenChanged: (albumId: string) => boolean;
		changeToCreateNewAlbum: () => void;
	}
	let { albumsViewData, isAlbumChosenChanged, albumIdToIsChosen, changeToCreateNewAlbum }: Props =
		$props();
</script>

<div class="all-albums-view">
	<div class="list">
		{#each albumsViewData as album}
			<div class="album">
				{album.name}
				{#if isAlbumChosenChanged(album.id)}
					*
				{/if}
				<DefaultCheckBox bind:checked={albumIdToIsChosen[album.id]} />
			</div>
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
