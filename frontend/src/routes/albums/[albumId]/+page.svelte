<script lang="ts">
	import PageLoadErrView from '$lib/components/PageLoadErrView.svelte';
	import VokiItemsGridContainer from '$lib/components/voki_item/VokiItemsGridContainer.svelte';
	import VokiItemView, {
		type VokiItemViewState
	} from '$lib/components/voki_item/VokiItemView.svelte';
	import type { PageProps } from './$types';

	let { data }: PageProps = $props();
	let vokis: VokiItemViewState[] = $state([]);
	function loadVokis() {
		if (data.response.isSuccess === false) {
			return;
		}
	}
	loadVokis();

	function getVokiViewItemState(vokiId: string): VokiItemViewState {}
	let;
</script>

{#if !data.response.isSuccess}
	<PageLoadErrView
		errs={data.response.errs}
		defaultMessage="Could not load album"
		additionalParams={[{ name: 'albumId', value: data.albumId }]}
	/>
{:else}
	<UserAlbumFilterAndSort />
	<VokiItemsGridContainer>
		{#each data.response.data.vokiIds as vokiId}
			<VokiItemView state={getVokiViewItemState(vokiId)} />
		{/each}
	</VokiItemsGridContainer>
{/if}
