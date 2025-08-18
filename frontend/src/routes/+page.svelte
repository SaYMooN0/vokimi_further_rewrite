<script lang="ts">
	import DefaultErrBlock from '$lib/components/errs/DefaultErrBlock.svelte';
	import VokiItemView from '$lib/components/VokiItemView.svelte';
	import { toast } from 'svelte-sonner';
	import type { PageProps } from './$types';
	import VokiItemsGridContainer from '$lib/components/VokiItemsGridContainer.svelte';
	import CatalogLoadingErrDisplay from './catalog/c_catalog_page/CatalogLoadingErrDisplay.svelte';

	let { data }: PageProps = $props();
</script>

{#if data.errs}
	<CatalogLoadingErrDisplay errs={data.errs} />
{:else}
	<VokiItemsGridContainer>
		{#each data.data.vokis as voki}
			<VokiItemView
				{voki}
				link={`/catalog/${voki.id}`}
				onMoreBtnClick={() => toast.error("Voki more button isn't implemented yet")}
			/>
		{/each}
	</VokiItemsGridContainer>
{/if}
