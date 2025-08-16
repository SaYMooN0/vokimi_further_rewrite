<script lang="ts">
	import DefaultErrBlock from '$lib/components/errs/DefaultErrBlock.svelte';
	import VokiItemsGridContainer from '$lib/components/VokiItemsGridContainer.svelte';
	import VokiItemView from '$lib/components/VokiItemView.svelte';
	import { toast } from 'svelte-sonner';
	import VokiSkeletonItem from '../draft/c_page/VokiSkeletonItem.svelte';
	import VokiUnableToLoad from '../draft/c_page/VokiUnableToLoad.svelte';
	import type { PageProps } from './$types';
	import { MyPublishedVokisCacheStore } from './my-published-vokis-cache-store.svelte';

	let { data }: PageProps = $props();
</script>

{#if data.errs}
	<DefaultErrBlock errList={data.errs} />
{:else}
	<VokiItemsGridContainer>
		{#await MyPublishedVokisCacheStore.EnsureExist(data.publishedVokiIds)}
			{#each data.publishedVokiIds as _}
				<VokiSkeletonItem />
			{/each}
		{:then _}
			{#if data.publishedVokiIds.length === 0}
				<h1>You don't have any published vokis</h1>
			{:else}
				{#each data.publishedVokiIds as vokiId}
					{#await MyPublishedVokisCacheStore.Get(vokiId)}
						<VokiSkeletonItem />
					{:then voki}
						{#if voki}
							<VokiItemView
								{voki}
								link={`/catalog/${vokiId}`}
								onMoreBtnClick={() => toast.error("Voki more button isn't implemented yet")}
							/>
						{:else}
							<VokiUnableToLoad {vokiId} />
						{/if}
					{/await}
				{/each}
			{/if}
		{/await}
	</VokiItemsGridContainer>
{/if}
