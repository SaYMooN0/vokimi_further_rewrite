<script lang="ts">
	import PrimaryButton from '$lib/components/buttons/PrimaryButton.svelte';
	import DefaultCheckBox from '$lib/components/inputs/DefaultCheckBox.svelte';
	import { ApiAlbums, RJO } from '$lib/ts/backend-communication/backend-services';
	import type { AlbumViewData } from '../add-voki-to-album-dialog-state.svelte';

	interface Props {
		albumsViewData: AlbumViewData[];
		albumIdToIsChosen: Record<string, boolean>;
		isAlbumChosenChanged: (albumId: string) => boolean;
		changeToCreateNewAlbum: () => void;
		updateVokiPresenceInAlbums: () => void;
	}
	let {
		albumsViewData,
		isAlbumChosenChanged,
		albumIdToIsChosen,
		changeToCreateNewAlbum,
		updateVokiPresenceInAlbums
	}: Props = $props();
</script>

<div class="all-albums-view">
	<div class="list">
		{#each albumsViewData as album}
			<label class="album unselectable">
				<svg
					class="album-icon"
					style="--icon-color-1: {album.mainColor}; --icon-color-2: {album.secondaryColor};"
					><use href="#{album.icon}" /></svg
				>
				{album.name}
				<DefaultCheckBox bind:checked={albumIdToIsChosen[album.id]} />
				<span class="changed-indicator" class:active={isAlbumChosenChanged(album.id)}></span>
			</label>
		{/each}
		<button class="create-new-btn" onclick={() => changeToCreateNewAlbum()}>
			Create new albums button
		</button>
	</div>
	<PrimaryButton onclick={() => updateVokiPresenceInAlbums()}>Save</PrimaryButton>
</div>

<style>
	.all-albums-view {
		display: grid;
		width: 100%;
		height: 100%;
		grid-template-rows: 1fr auto;
		justify-items: center;
	}

	.list {
		display: flex;
		flex-direction: column;
		height: 100%;
		gap: 0.125rem;
		width: 100%;
	}
	.album {
		display: grid;
		grid-template-columns: auto 1fr auto auto;
		gap: 0.5rem;
		align-items: center;
		font-size: 1.125rem;
		font-weight: 450;
		padding: 0.25rem 1rem;
		border-radius: 0.375rem;
	}
	.album:hover {
		background-color: var(--secondary);
	}
	.album-icon {
		height: 1.675rem;
		width: 1.675rem;
		stroke-width: 1.9;
	}
	.changed-indicator {
		height: 0.375rem;
		width: 0.375rem;
		border-radius: 50%;
		transform: scale(0);
		transition: all 0.15s ease-out;
	}
	.changed-indicator.active {
		background-color: var(--primary);
		transform: scale(1);
	}
	.create-new-btn {
		margin-top: 1rem;
		align-self: center;
		width: fit-content;
		padding: 0.25rem 1rem;
		border: none;
		background-color: var(--secondary);
		color: var(--secondary-foreground);
		border-radius: 0.5rem;
		font-weight: 500;
		letter-spacing: 0.1px;
		transition: all 0.1s ease-out;
	}
	.create-new-btn:hover {
		background-color: var(--accent);
		color: var(--accent-foreground);
		letter-spacing: 0.25px;
	}
</style>
