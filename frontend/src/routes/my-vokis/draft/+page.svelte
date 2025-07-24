<script lang="ts">
	import { goto } from '$app/navigation';
	import DefaultErrBlock from '$lib/components/errs/DefaultErrBlock.svelte';
	import { StringUtils } from '$lib/ts/utils/string-utils';
	import { toast } from 'svelte-sonner';
	import { MyVokisCacheStore } from '../my-vokis-cache-store.svelte';
	import type { PageProps } from './$types';
	import VokiSkeletonItem from './c_page/VokiSkeletonItem.svelte';
	import VokiUnableToLoad from './c_page/VokiUnableToLoad.svelte';
	import { StorageBucketMain } from '$lib/ts/backend-communication/storage-buckets';

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
				<h1>No vokis created</h1>
			{:else}
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
								<img
									class="voki-cover"
									src={StorageBucketMain.fileSrc(voki.cover)}
									alt="voki cover"
								/>
								<div class="bottom-items">
									<div class="name-line">
										<p class="voki-name">
											{voki?.name}
										</p>
										<svg
											class="voki-more-btn interactable"
											onclick={(e) => {
												e.preventDefault();
												toast.error("Voki more button isn't implemented yet");
											}}
										>
											<use href="#common-more-icon" />
										</svg>
									</div>
									<div class="authors">
										by: <span
											class="primary-author-span interactable"
											onclick={(e) => {
												e.preventDefault();
												goto(`/user/${voki.primaryAuthorId}`);
											}}>{voki.primaryAuthorId}</span
										>
										{#if voki.coAuthorsCount > 0}
											<div
												class="co-authors interactable"
												onclick={(e) => {
													e.preventDefault();
													toast.error(
														'You cannot see co-authors here yet. Please open voki creation page'
													);
												}}
											>
												+ {voki.coAuthorsCount}
											</div>
										{/if}
									</div>
								</div>
							</a>
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

	.voki-item {
		display: flex;
		flex-direction: column;
		gap: var(--voki-cover-name-gap);
		width: 100%;
		border-radius: var(--voki-cover-border-radius);
		cursor: pointer;
	}

	.voki-item:not(:has(.interactable:hover)):active {
		background-color: var(--secondary);
	}

	.voki-cover {
		width: 100%;
		border-radius: var(--voki-cover-border-radius);
		aspect-ratio: var(--voki-cover-aspect-ratio);
	}

	.bottom-items {
		padding: 0 0 0.25rem;
	}

	.name-line {
		display: grid;
		grid-template-columns: 1fr auto;
		align-items: start;
	}

	.voki-name {
		display: flex;
		display: -webkit-box;
		flex-direction: row;
		color: var(--text);
		font-size: 1.125rem;
		font-weight: 420;
		line-height: calc(var(--voki-name-max-height) / 2);
		letter-spacing: 0.12px;
		-webkit-line-clamp: 2;
		line-clamp: 2;
		-webkit-box-orient: vertical;
		text-overflow: ellipsis;
		overflow: hidden;
	}

	.voki-item:not(:has(.interactable:hover)):hover .voki-name {
		text-decoration: underline;
		text-decoration-thickness: 2px;
	}

	.voki-more-btn {
		height: calc(var(--voki-name-max-height) * 0.55);
		border-radius: 0.25rem;
		color: var(--text);
		aspect-ratio: 1/1;
		stroke-width: 3.2;
	}

	.voki-more-btn:hover {
		background-color: var(--muted);
	}

	.authors {
		display: grid;
		align-items: center;
		color: var(--secondary-foreground);
		font-size: 0.875rem;
		overflow: hidden;
		grid-template-columns: auto 1fr auto;
	}

	.primary-author-span {
		margin-left: 0.25rem;
		overflow: hidden;
		white-space: nowrap;
		text-overflow: ellipsis;
		color: var(--primary);
		font-weight: 450;
	}

	.primary-author-span:hover {
		text-decoration: underline;
	}

	.co-authors {
		padding: 0 0.25rem;
		margin: 0.125rem 0.25rem 0.125rem 0;
		border-radius: 0.25rem;
		font-weight: 440;
		letter-spacing: -1.2px;
		box-shadow: var(--shadow);
		transition: all 0.06s ease-in;
	}

	.co-authors:hover {
		background-color: var(--secondary);
	}
</style>
