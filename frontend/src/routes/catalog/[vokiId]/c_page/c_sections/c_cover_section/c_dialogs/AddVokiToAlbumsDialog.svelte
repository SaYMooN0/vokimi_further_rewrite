<script lang="ts">
	import ReloadButton from '$lib/components/buttons/ReloadButton.svelte';
	import DialogWithCloseButton from '$lib/components/dialogs/DialogWithCloseButton.svelte';
	import DefaultErrBlock from '$lib/components/errs/DefaultErrBlock.svelte';
	import CubesLoader from '$lib/components/loaders/CubesLoader.svelte';
	import { AddVokiToAlbumsDialogState } from './add-voki-to-album-dialog-state.svelte';
	import AlbumsDialogAlbumsChoosing from './c_albums_dialog/AlbumsDialogAlbumsChoosing.svelte';
	import AlbumsDialogNoAlbumsState from './c_albums_dialog/AlbumsDialogNoAlbumsState.svelte';

	interface Props {
		vokiId: string;
	}
	let { vokiId }: Props = $props();

	let dialog = $state<DialogWithCloseButton>()!;
	let dialogState = new AddVokiToAlbumsDialogState(vokiId);

	export function open() {
		dialogState.ensureFresh();
		dialog.open();
	}
	function changeToCreateNewAlbum() {}
</script>

<DialogWithCloseButton bind:this={dialog} dialogId="user-voki-albums-dialog">
	{#if dialogState.albumsState.name === 'loading'}
		<div class="loading-container">
			<CubesLoader sizeRem={6} />
			<h1>Loading your albums</h1>
		</div>
	{:else if dialogState.albumsState.name === 'errs'}
		<div class="err-view">
			<h1>Something went wrong</h1>
			<DefaultErrBlock errList={dialogState.albumsState.errs} />
		</div>
	{:else if dialogState.albumsState.name === 'ok'}
		{#if dialogState.albumsState.albums.length === 0}
			<AlbumsDialogNoAlbumsState {changeToCreateNewAlbum} />
		{:else}
			<AlbumsDialogAlbumsChoosing
				{changeToCreateNewAlbum}
				albumsViewData={dialogState.albumsState.albums}
				albumIdToIsChosen={dialogState.albumToIsChosen}
				isAlbumChosenChanged={dialogState.isAlbumChosenChanged}
			/>
		{/if}
	{:else}
		<h1>Something went wrong</h1>
		<ReloadButton onclick={() => dialogState.updateForce()} />
	{/if}
</DialogWithCloseButton>

<style>
	:global(#user-voki-albums-dialog > .dialog-content) {
		height: 32rem;
		width: 48rem;
	}
	.loading-container {
		display: flex;
		flex-direction: column;
		justify-content: center;
		align-items: center;
		gap: 2rem;
		width: 100%;
		height: 100%;
		padding-bottom: 2rem;
	}

	.loading-container > h1 {
		color: var(--secondary-foreground);
		font-size: 2rem;
		font-weight: 550;
		letter-spacing: 0.5px;
	}
	.err-view {
		width: 100%;
		height: 100%;
	}
</style>
