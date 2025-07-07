<script lang="ts">
	import { goto } from '$app/navigation';
	import DefaultErrBlock from '$lib/components/errs/DefaultErrBlock.svelte';
	import { StringUtils } from '$lib/ts/string-utils';
	import { MyVokisCacheStore } from '../my-vokis-cache-store.svelte';
	import type { PageProps } from './$types';
	import VokiSkeletonItem from './c_page.svelte/VokiSkeletonItem.svelte';
	import VokiUnableToLoad from './c_page.svelte/VokiUnableToLoad.svelte';

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
			{#each data.draftVokiIds as vokiId}
				{#await MyVokisCacheStore.Get(vokiId)}
					<VokiSkeletonItem />
				{:then voki}
					{#if voki === null || voki === undefined}
						<VokiUnableToLoad {vokiId} />
					{:else}
						<a
							href="/voki-creation/{StringUtils.pascalToKebab(voki.type)}/{vokiId}"
							class="voki-item"
						>
							<div class="voki-cover"></div>
							<div class="bottom-items">
								<p class="voki-name">
									{voki?.name}
								</p>
								<div class="authors">
									<p class="primary-author">
										Primary author: <span
											class="primary-author-span"
											onclick={(e) => {
												e.preventDefault();
												goto(`/user/${voki.primaryAuthorId}`);
											}}>{voki.primaryAuthorId}</span
										>
									</p>
									{#if voki.coAuthorsCount > 0}
										<p class="co-authors">{voki.coAuthorsCount} Co-Authors</p>
									{:else}
										<p class="co-authors">No Co-Authors</p>
									{/if}
								</div>
							</div>
						</a>
					{/if}
				{/await}
			{/each}
		{/await}
	</div>
{/if}

<style>
	.vokis-container {
		display: flex;
		gap: 1rem;
		display: grid;
		grid-template-columns: repeat(auto-fill, minmax(15rem, 1fr));
		--voki-cover-aspect-ratio: 1.5/1;
		--voki-name-max-height: 2.4rem;
		--voki-cover-name-gap: 0.5rem;
		--voki-cover-border-radius: 0.75rem;
	}

	.voki-item {
		width: 100%;
		display: flex;
		flex-direction: column;
		gap: var(--voki-cover-name-gap);
		cursor: pointer;
		border-radius: var(--voki-cover-border-radius);
	}

	.voki-item:not(:has(.primary-author-span:hover)):active {
		background-color: var(--secondary);
	}
	.voki-cover {
		width: 100%;
		aspect-ratio: var(--voki-cover-aspect-ratio);
		background-color: var(--muted);
		border-radius: var(--voki-cover-border-radius);
	}
	.bottom-items {
		padding: 0 0.25rem 0.25rem 0.25rem;
	}
	.voki-name {
		-webkit-line-clamp: 2;
		display: -webkit-box;
		display: box;
		-webkit-box-orient: vertical;
		text-overflow: ellipsis;
		overflow: hidden;
		line-height: calc(var(--voki-name-max-height) / 2);
		font-weight: 420;
		max-height: var(--voki-name-max-height);
		font-size: 1.125rem;
		letter-spacing: 0.12px;
		color: var(--text);
	}
	.voki-item:not(:has(.primary-author-span:hover)):hover .voki-name {
		text-decoration: underline;
		text-decoration-thickness: 2px;
	}
	.authors p {
		font-size: 0.875rem;
		color: var(--secondary-foreground);
		overflow: hidden;
		display: block;
	}

	.primary-author {
		white-space: nowrap;
	}

	.primary-author-span {
		color: var(--primary);
		font-weight: 450;
	}
	.primary-author-span:hover {
		text-decoration: underline;
	}
	.co-authors {
		white-space: nowrap;

		text-overflow: ellipsis;
	}
</style>
