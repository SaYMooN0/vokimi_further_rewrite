<script lang="ts">
	import AlbumPageHeader from '../../c_pages_shared/AlbumPageHeader.svelte';
	import AutoAlbumsNoAlbumsMessage from '../c_shared/AutoAlbumsNoAlbumsMessage.svelte';
	import type { PageProps } from './$types';

	let { data }: PageProps = $props();
	function vokisWithLastTakenDate() {
		if (data.response.isSuccess === false) {
			return [];
		}

		return Object.entries(data.response.data.vokiIdWithLastTakenDate).sort(
			(a, b) => b[1].getTime() - a[1].getTime()
		);
	}
	let sortedAndFilteredVokis = $state(vokisWithLastTakenDate());
	const initialCount = data.response.isSuccess
		? Object.keys(data.response.data.vokiIdWithLastTakenDate).length
		: 0;
</script>

{#if initialCount === 0}
	<AutoAlbumsNoAlbumsMessage
		albumName="taken Vokis"
		howToAddVokis="Vokis will be added automatically to this album after you take them for the first time"
	/>
{:else}
	<AlbumPageHeader content={{ type: 'auto', albumName: 'taken Vokis' }} />
	<div class="albums-container">
		{#each sortedAndFilteredVokis as voki}
			<div>
				{voki[0]}
			</div>
		{/each}
	</div>
{/if}
