<script lang="ts">
	import PageLoadErrView from '$lib/components/PageLoadErrView.svelte';
	import VokiItemsGridContainer from '$lib/components/voki_item/VokiItemsGridContainer.svelte';
	import VokiItemView, {
		type VokiItemViewState
	} from '$lib/components/voki_item/VokiItemView.svelte';
	import type { VokiType } from '$lib/ts/voki-type';
	import { toast } from 'svelte-sonner';
	import AlbumPageFilterAndSort from '../c_pages_shared/AlbumPageFilterAndSort.svelte';
	import type { PageProps } from './$types';
	import AlbumPageHeader from '../c_pages_shared/AlbumPageHeader.svelte';
	import { SvelteSet } from 'svelte/reactivity';

	let { data }: PageProps = $props();
	let allLoadedVokis: VokiItemViewState[] = $state([]);
	let sortedAndFilteredVokis: () => VokiItemViewState[] = $derived(() => {
		return [];
	});
	function loadVokis() {
		if (data.response.isSuccess === false) {
			return;
		}
		// allLoadedVokis = data.response.data.vokiIds.map((id) => {
		// 	PublishedVokisStore.Get(id);
		// });
		 
	}
	loadVokis();

	const allSortOptions: string[] = ['From A to Z', 'From Z to A', 'Newest', 'Oldest'];
	let filterAndSort: {
		chosenVokiTypes: Set<VokiType>;
		currentSortOption: string;
	} = $state({ chosenVokiTypes: new SvelteSet<VokiType>(), currentSortOption: 'Newest' });
	function chooseSortOption(opt: string) {
		if (allSortOptions.includes(opt)) {
			filterAndSort.currentSortOption = opt;
		} else {
			toast.error('Unknown sort option');
		}
	}
	function onVokiTypeClick(vokiType: VokiType) {
		if (filterAndSort.chosenVokiTypes.has(vokiType)) {
			filterAndSort.chosenVokiTypes.delete(vokiType);
		} else {
			filterAndSort.chosenVokiTypes.add(vokiType);
		}
		console.log(filterAndSort.chosenVokiTypes.has(vokiType));
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
		{chooseSortOption}
		sortOptions={allSortOptions}
		chosenVokiTypes={filterAndSort.chosenVokiTypes}
		currentSortOption={filterAndSort.currentSortOption}
	/>
	<VokiItemsGridContainer>
		{#each sortedAndFilteredVokis() as voki}
			<VokiItemView state={voki} />
		{/each}
	</VokiItemsGridContainer>
{/if}
