<script lang="ts">
	import PageLoadErrView from '$lib/components/PageLoadErrView.svelte';
	import VokiItemsGridContainer from '$lib/components/voki_item/VokiItemsGridContainer.svelte';
	import VokiItemView from '$lib/components/voki_item/VokiItemView.svelte';
	import type { VokiType } from '$lib/ts/voki-type';
	import AlbumPageFilterAndSort from '../c_pages_shared/AlbumPageFilterAndSort.svelte';
	import type { PageProps } from './$types';
	import AlbumPageHeader from '../c_pages_shared/AlbumPageHeader.svelte';
	import { AlbumPageState } from './album-page-state.svelte';
	import { toast } from 'svelte-sonner';

	let { data }: PageProps = $props();
	const pageState = new AlbumPageState(
		data.response.isSuccess ? data.response.data.vokiIds : [],
		(e) => toast.error("notification error: " + e)
	);

	function onVokiTypeClick(vokiType: VokiType) {
		if (pageState.filterAndSort.chosenVokiTypes.has(vokiType)) {
			pageState.filterAndSort.chosenVokiTypes.delete(vokiType);
		} else {
			pageState.filterAndSort.chosenVokiTypes.add(vokiType);
		}
	}
</script>

{#if !data.response.isSuccess}
	<PageLoadErrView
		errs={data.response.errs}
		defaultMessage="Could not load album"
		additionalParams={[{ name: 'albumId', value: data.albumId }]}
	/>
{:else}
	<AlbumPageHeader
		content={{
			type: 'user',
			icon: {
				href: data.response.data.icon,
				mainColor: data.response.data.mainColor,
				secondaryColor: data.response.data.secondaryColor
			},
			albumName: data.response.data.name
		}}
	/>
	<AlbumPageFilterAndSort
		{onVokiTypeClick}
		chooseSortOption={(o) => pageState.chooseSortOption(o)}
		sortOptions={pageState.allSortOptions}
		chosenVokiTypes={pageState.filterAndSort.chosenVokiTypes}
		currentSortOption={pageState.filterAndSort.currentSortOption}
	/>
	<VokiItemsGridContainer>
		{#each pageState.sortedAndFilteredVokis() as voki}
			<VokiItemView state={voki} />
		{/each}
	</VokiItemsGridContainer>
{/if}
