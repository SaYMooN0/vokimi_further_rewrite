<script lang="ts">
	import AutoAlbumPageHeader from '../c_shared/AutoAlbumPageHeader.svelte';
	import AutoAlbumsNoAlbumsMessage from '../c_shared/AutoAlbumsNoAlbumsMessage.svelte';
	import type { PageProps } from './$types';

	let { data }: PageProps = $props();
	function vokisWithLastTakenDate() {
		if (data.response.isSuccess === false) {
			return [];
		}

		return Object.entries(data.response.data.vokiIdWithRatingDate).sort(
			(a, b) => b[1].getTime() - a[1].getTime()
		);
	}
	let sortedAndFilteredVokis = $state(vokisWithLastTakenDate());
	const initialCount = data.response.isSuccess
		? Object.keys(data.response.data.vokiIdWithRatingDate).length
		: 0;
</script>

{#if initialCount === 0}
	<AutoAlbumsNoAlbumsMessage
		albumName={data.albumName}
		howToAddVokis="Vokis will be added automatically to this album after you rate them for the first time"
	/>
{:else}
	<AutoAlbumPageHeader albumName={data.albumName} />
	<div class="albums-container">
		{#each sortedAndFilteredVokis as voki}
			<div>
				{voki[0]}
			</div>
		{/each}
	</div>
{/if}
