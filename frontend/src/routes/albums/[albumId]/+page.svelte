<script lang="ts">
	import PageLoadErrView from '$lib/components/PageLoadErrView.svelte';
	import VokiItemsGridContainer from '$lib/components/voki_item/VokiItemsGridContainer.svelte';
	import VokiItemView from '$lib/components/voki_item/VokiItemView.svelte';
	import type { PageProps } from './$types';

	let { data }: PageProps = $props();
</script>

{#if !data.response.isSuccess}
	<PageLoadErrView
		errs={data.response.errs}
		defaultMessage="Could not load album"
		additionalParams={[{ name: 'albumId', value: data.albumId }]}
	/>
{:else}
	<VokiItemsGridContainer>
		{#each data.response.data.vokiIds as vokiId}
			<VokiItemView state={pageState.getVokiViewItemState(vokiId)} />
		{/each}
	</VokiItemsGridContainer>
{/if}
