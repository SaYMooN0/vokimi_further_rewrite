<script lang="ts">
	import AlbumPageHeader from '../../c_pages_shared/AlbumPageHeader.svelte';
	import AutoAlbumsNoAlbumsMessage from '../../c_pages_shared/AlbumEmptyMessage.svelte';
	import type { PageProps } from './$types';

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
