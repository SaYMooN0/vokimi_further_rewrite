<script lang="ts">
	import type { VokiAlbumPreviewData } from '../types';
	import AlbumsPageSectionHeader from './AlbumsPageSectionHeader.svelte';

	interface Props {
		albums: VokiAlbumPreviewData[];
	}
	let { albums } = $props();
</script>

{#snippet headerIcon()}
	<svg> <use href="#common-plus-icon" /> </svg>
{/snippet}

<AlbumsPageSectionHeader
	headerText="User albums"
	rightButton={{
		text: 'Create new',
		onclick: () => {},
		icon: headerIcon
	}}
/>

<div>
	{#each albums as album}{/each}
</div>
