<script lang="ts">
	import VokiItemView from '$lib/components/VokiItemView.svelte';
	import { toast } from 'svelte-sonner';
	import type { PageProps } from './$types';
	import VokiItemsGridContainer from '$lib/components/VokiItemsGridContainer.svelte';
	import PageLoadErrView from '$lib/components/PageLoadErrView.svelte';

	let { data }: PageProps = $props();
</script>

{#if data.errs}
	<PageLoadErrView errs={data.errs} defaultMessage="Could not load vokis catalog" />
{:else if data.data.vokis.length === 0}
	<h1>Voki catalog is empty</h1>
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
