<script lang="ts">
	import PrimaryButton from '$lib/components/buttons/PrimaryButton.svelte';
	import { getCreateNewAlbumOpenFunction } from '../../c_layout/ts_layout_contexts/album-creation-dialog-context';
	import type { VokiAlbumPreviewData } from '../types';
	import AlbumsPageSectionHeader from './AlbumsPageSectionHeader.svelte';

	interface Props {
		albums: VokiAlbumPreviewData[];
	}
	let { albums } = $props();
	const openCreateNewAlbumDialog = getCreateNewAlbumOpenFunction();
</script>

{#snippet headerIcon()}
	<svg> <use href="#common-plus-icon" /> </svg>
{/snippet}

<AlbumsPageSectionHeader
	headerText="User albums"
	rightButton={{
		text: 'Create new album',
		onclick: () => openCreateNewAlbumDialog(),
		icon: headerIcon
	}}
/>
{#if albums.length === 0}
	<div class="no-albums-message">
		<p>You have no albums</p>
		<PrimaryButton onclick={() => openCreateNewAlbumDialog()}>Create first album</PrimaryButton>
	</div>
{:else}
	<div>
		{#each albums as album}
			<div>
				{album.name}
			</div>
		{/each}
	</div>
{/if}
