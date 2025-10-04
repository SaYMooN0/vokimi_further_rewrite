<script lang="ts">
	import type { AutoAlbumsColorsPair } from '../types';
	import AlbumsPageSectionHeader from './AlbumsPageSectionHeader.svelte';
	import AutoAlbumLink from './c_auto_albums_section/AutoAlbumLink.svelte';

	interface Props {
		takenVokisAlbums: AutoAlbumsColorsPair;
		ratedVokisAlbums: AutoAlbumsColorsPair;
		commentedVokisAlbums: AutoAlbumsColorsPair;
	}
	let { takenVokisAlbums, ratedVokisAlbums, commentedVokisAlbums }: Props = $props();
</script>

{#snippet headerIcon()}
	<h1>icon</h1>
{/snippet}

<AlbumsPageSectionHeader
	headerText="Auto albums"
	rightButton={{
		text: 'Edit Appearance',
		onclick: () => {},
		icon: headerIcon
	}}
/>

<div class="auto-albums-container">
	<AutoAlbumLink
		href="/albums/taken-vokis"
		text="Taken Vokis"
		iconId="#albums-taken-vokis-icon"
		iconMainColor={takenVokisAlbums.mainColor}
		iconSecondaryColor={takenVokisAlbums.secondaryColor}
	/>
	<AutoAlbumLink
		href="/albums/rated-vokis"
		text="Rated Vokis"
		iconId="#albums-rated-vokis-icon"
		iconMainColor={ratedVokisAlbums.mainColor}
		iconSecondaryColor={ratedVokisAlbums.secondaryColor}
	/>
	<AutoAlbumLink
		href="/albums/commented-vokis"
		text="Commented Vokis"
		iconId="#albums-commented-vokis-icon"
		iconMainColor={commentedVokisAlbums.mainColor}
		iconSecondaryColor={commentedVokisAlbums.secondaryColor}
	/>
</div>

<style>
</style>
