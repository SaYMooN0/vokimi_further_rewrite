<script lang="ts">
	import DefaultErrBlock from '$lib/components/errs/DefaultErrBlock.svelte';
	import { StringUtils } from '$lib/ts/utils/string-utils';
	import { toast } from 'svelte-sonner';
	import { MyVokisCacheStore } from '../my-vokis-cache-store.svelte';
	import type { PageProps } from './$types';
	import VokiSkeletonItem from './c_page/VokiSkeletonItem.svelte';
	import VokiUnableToLoad from './c_page/VokiUnableToLoad.svelte';
	import VokiItemView from '$lib/components/VokiItemView.svelte';
	let { data }: PageProps = $props();
</script>

{#if data.errs}
	<DefaultErrBlock errList={data.errs} />
{:else}
	<div class="vokis-container">
		{#await MyVokisCacheStore.EnsureExist(data.draftVokiIds)}
			{#each data.draftVokiIds as _}
				<VokiSkeletonItem />
			{/each}
		{:then _}
			{#if data.draftVokiIds.length === 0}
				<h1>You don't have any draft vokis</h1>
			{:else}
				{#each data.draftVokiIds as vokiId}
					{#await MyVokisCacheStore.Get(vokiId)}
						<VokiSkeletonItem />
					{:then voki}
						{#if voki}
							<VokiItemView
								{voki}
								link={`/voki-creation/${StringUtils.pascalToKebab(voki.type)}/${vokiId}`}
								onMoreBtnClick={() => toast.error("Voki more button isn't implemented yet")}
							/>
						{:else}
							<VokiUnableToLoad {vokiId} />
						{/if}
					{/await}
				{/each}
			{/if}
		{/await}
	</div>
{/if}

<style>
	.vokis-container {
		display: flex;
		display: grid;
		gap: 1rem;
		grid-template-columns: repeat(auto-fill, minmax(15rem, 1fr));

		--voki-name-max-height: 2.4rem;
		--voki-cover-name-gap: 0.5rem;
		--voki-cover-border-radius: 0.75rem;
	}
</style>
