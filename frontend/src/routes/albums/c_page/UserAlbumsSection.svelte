<script lang="ts">
	import { goto } from '$app/navigation';
	import PrimaryButton from '$lib/components/buttons/PrimaryButton.svelte';
	import { getCreateNewAlbumOpenFunction } from '../../c_layout/ts_layout_contexts/album-creation-dialog-context';
	import type { VokiAlbumPreviewData } from '../types';
	import AlbumsPageSectionHeader from './AlbumsPageSectionHeader.svelte';
	import ConfirmAlbumDeletionDIalog from './c_user_albums_section/ConfirmAlbumDeletionDIalog.svelte';
	import EditAlbumDialog from './c_user_albums_section/EditAlbumDialog.svelte';
	import UserAlbumsContextMenu from './c_user_albums_section/UserAlbumsContextMenu.svelte';
	import UserAlbumView from './c_user_albums_section/UserAlbumView.svelte';

	interface Props {
		albums: VokiAlbumPreviewData[];
	}
	let { albums }: Props = $props();
	const openCreateNewAlbumDialog = getCreateNewAlbumOpenFunction();

	function openNewAlbumDialog(): void {
		const onAfterCreated = (newAlbumId: string) => {
			goto(`/albums/${newAlbumId}`);
		};
		openCreateNewAlbumDialog(onAfterCreated);
	}
	let userAlbumsContextMenu = $state<UserAlbumsContextMenu>()!;
	let editAlbumDialog = $state<EditAlbumDialog>()!;
	let confirmAlbumDeletionDialog = $state<ConfirmAlbumDeletionDIalog>()!;
</script>

{#snippet headerIcon()}
	<svg><use href="#common-plus-icon" /></svg>
{/snippet}

<AlbumsPageSectionHeader
	headerText="User albums"
	rightButton={{
		text: 'Create new album',
		onclick: openNewAlbumDialog,
		icon: headerIcon
	}}
/>
<ConfirmAlbumDeletionDIalog bind:this={confirmAlbumDeletionDialog} />
<EditAlbumDialog bind:this={editAlbumDialog} />
<UserAlbumsContextMenu
	bind:this={userAlbumsContextMenu}
	openDeleteAlbumDialog={(a) => confirmAlbumDeletionDialog.open(a)}
	openEditAlbumDialog={(a) => editAlbumDialog.open(a)}
/>
{#if albums.length === 0}
	<div class="no-albums-message">
		<p>You have no albums</p>
		<PrimaryButton onclick={() => openNewAlbumDialog()}>Create first album</PrimaryButton>
	</div>
{:else}
	<div class="albums-list">
		{#each albums as album}
			<UserAlbumView
				{album}
				openContextMenu={(event, album) => userAlbumsContextMenu.open(event, album)}
			/>
		{/each}
	</div>
{/if}

<style>
	.albums-list {
		display: flex;
		flex-direction: column;
		gap: 0.5rem;
	}
</style>
