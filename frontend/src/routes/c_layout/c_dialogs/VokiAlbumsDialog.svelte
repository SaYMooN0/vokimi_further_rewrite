<script lang="ts">
	import AuthView from '$lib/components/AuthView.svelte';
	import DialogWithCloseButton from '$lib/components/dialogs/DialogWithCloseButton.svelte';
	import type { Err } from '$lib/ts/err';
	import AlbumsDialogCreateNewState from './c_albums_dialog/AlbumsDialogCreateNewState.svelte';
	import AlbumsDialogErrView from './c_albums_dialog/AlbumsDialogErrView.svelte';
	import AlbumsDialogLoadingState from './c_albums_dialog/AlbumsDialogLoadingState.svelte';
	import AlbumsDialogSignInRequiredState from './c_albums_dialog/AlbumsDialogSignInRequiredState.svelte';
	import AlbumsDialogViewAllState from './c_albums_dialog/AlbumsDialogViewAllState.svelte';

	let dialog = $state<DialogWithCloseButton>()!;

	type AlbumsDialogState = 'albums' | 'create-new' | 'loading';

	let dialogState = $state<AlbumsDialogState>('albums');
	let loadingErr = $state<Err | null>(null);
	
    export function open() {
		dialogState = 'albums';
		dialog.open();
	}
	async function refresh() {
		dialogState = 'loading';
		loadingErr = null;
	
    }
</script>

<DialogWithCloseButton
	bind:this={dialog}
	dialogId="user-voki-albums-dialog"
	subheading={dialogState === 'create-new' ? 'Create new album' : undefined}
>
	<div>
		<AuthView>
			{#snippet authenticated()}
				{#if dialogState === 'albums'}
					<AlbumsDialogViewAllState />
				{:else if dialogState === 'create-new'}
					<AlbumsDialogCreateNewState />
				{:else if dialogState === 'loading'}
					<AlbumsDialogLoadingState />
				{:else}
					<AlbumsDialogErrView err={loadingErr} />
				{/if}
			{/snippet}
			{#snippet unauthenticated()}
				<AlbumsDialogSignInRequiredState />
			{/snippet}
		</AuthView>
		if not auth then AlbumsBlockedSignInRequiredDIalog else refresh button err msg albums list no created
		albums
	</div>
</DialogWithCloseButton>

<style>
	:global(#user-voki-albums-dialog > .dialog-content) {
		height: 20rem;
		width: 40rem;
		animation: var(--default-fade-in-animation);
	}
</style>
