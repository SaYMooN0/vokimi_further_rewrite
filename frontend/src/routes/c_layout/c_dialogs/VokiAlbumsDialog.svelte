<script lang="ts">
	import AuthView from '$lib/components/AuthView.svelte';
	import DialogWithCloseButton from '$lib/components/dialogs/DialogWithCloseButton.svelte';
	import type { Err } from '$lib/ts/err';
	import { albumsStore, type VokisAlbumData } from '$lib/ts/stores/user-albums-store.svelte';
	import AlbumsDialogCreateNewState from './c_albums_dialog/AlbumsDialogCreateNewState.svelte';
	import AlbumsDialogErrView from './c_albums_dialog/AlbumsDialogErrView.svelte';
	import AlbumsDialogLoadingState from './c_albums_dialog/AlbumsDialogLoadingState.svelte';
	import AlbumsDialogSignInRequiredState from './c_albums_dialog/AlbumsDialogSignInRequiredState.svelte';
	import AlbumsDialogViewAllState from './c_albums_dialog/AlbumsDialogViewAllState.svelte';
	import AlbumsDialogNoAlbumsState from './c_albums_dialog/AlbumsDialogNoAlbumsState.svelte';

	let dialog = $state<DialogWithCloseButton>()!;

	type AlbumsDialogState = 'albums' | 'create-new' | 'loading';

	let currentOperatingVokiId = $state<string | null>(null);
	let dialogState = $state<AlbumsDialogState>('albums');
	let loadingErr = $state<Err | null>(null);
	let albums = $state<VokisAlbumData[]>([]);

	export function open(vokiId: string) {
		dialogState = 'loading';
		currentOperatingVokiId = vokiId;
		dialog.open();
		refresh();
	}
	async function refresh() {
		dialogState = 'loading';
		loadingErr = null;
		albumsStore.getAlbums().then((res) => {
			if (res) {
				albums = res;
				dialogState = 'albums';
			} else {
				loadingErr = { message: 'Failed to load albums' };
				albums = [];
			}
		});
	}
	function onClose() {
		currentOperatingVokiId = null;
	}
	function dialogSubheading() {
		if (dialogState === 'create-new') {
			return 'Create new album';
		}
		if (dialogState === 'albums' && albums.length === 0) {
			return 'Start organizing your Vokis with albums';
		}
		return undefined;
	}
</script>

<DialogWithCloseButton
	bind:this={dialog}
	dialogId="user-voki-albums-dialog"
	subheading={dialogSubheading()}
	onBeforeClose={() => onClose()}
>
	<AuthView>
		{#snippet authenticated()}
			{#if dialogState === 'loading'}
				<AlbumsDialogLoadingState />
			{:else if loadingErr}
				<AlbumsDialogErrView err={loadingErr} />
			{:else if dialogState === 'create-new'}
				<AlbumsDialogCreateNewState />
			{:else if dialogState === 'albums' && albums.length === 0}
				<AlbumsDialogNoAlbumsState createNew={() => (dialogState = 'create-new')} />
			{:else if dialogState === 'albums'}
				<AlbumsDialogViewAllState
					{albums}
					changeStateToCreateNew={() => (dialogState = 'create-new')}
				/>
			{:else}
				<AlbumsDialogErrView err={{ message: 'Unexpected error' }} />
			{/if}
		{/snippet}
		{#snippet unauthenticated()}
			<AlbumsDialogSignInRequiredState closeDialog={() => dialog.close()} />
		{/snippet}
	</AuthView>
</DialogWithCloseButton>

<style>
	:global(#user-voki-albums-dialog > .dialog-content) {
		width: 44rem;
		height: 24rem;
		animation: var(--default-fade-in-animation);
	}
</style>
