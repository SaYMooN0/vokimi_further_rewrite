<script lang="ts">
	import AlbumPageHeader from '../../c_pages_shared/AlbumPageHeader.svelte';
	import type { PageProps } from './$types';
	import { TakenVokisAlbumPageState } from './taken-vokis-album-page-state.svelte';
	import PageLoadErrView from '$lib/components/PageLoadErrView.svelte';
	import AlbumPageFilterAndSort from '../../c_pages_shared/AlbumPageFilterAndSort.svelte';
	import VokiItemView from '$lib/components/voki_item/VokiItemView.svelte';
	import AlbumEmptyMessage from '../../c_pages_shared/AlbumEmptyMessage.svelte';
	import NoVokisInAlbumMatchFilterMessage from '../../c_pages_shared/NoVokisInAlbumMatchFilterMessage.svelte';
	import BasicUserDisplay from '$lib/components/BasicUserDisplay.svelte';
	import { relativeTime } from 'svelte-relative-time';

	let { data }: PageProps = $props();

	const pageState = new TakenVokisAlbumPageState(
		data.response.isSuccess ? data.response.data.vokis : {},
		(_, __) => {}
	);
</script>

{#if !data.response.isSuccess}
	<PageLoadErrView
		errs={data.response.errs}
		defaultMessage="Could not load taken Vokis auto album"
	/>
{:else}
	<AlbumPageHeader
		content={{
			type: 'auto',
			albumName: 'taken Vokis'
		}}
	/>

	{#if pageState.isInitialListEmpty()}
		<AlbumEmptyMessage
			title="Your taken Vokis auto album is empty"
			subtitle="Vokis will be added automatically to this album after you complete them for the first time"
		/>
	{:else}
		<AlbumPageFilterAndSort
			onVokiTypeClick={(t) => pageState.toggleTypeFilter(t)}
			chooseSortOption={(o) => pageState.chooseSortOption(o)}
			sortOptions={pageState.allSortOptions}
			chosenVokiTypes={pageState.filterAndSort.chosenVokiTypes}
			currentSortOption={pageState.filterAndSort.currentSortOption}
		/>

		{#if pageState.sortedAndFilteredVokis().length === 0}
			<NoVokisInAlbumMatchFilterMessage />
		{:else}
			<div class="vokis-container">
				{#each pageState.sortedAndFilteredVokis() as voki}
					<div class="voki-item">
						<VokiItemView state={{ ...voki, hide: ['Name', 'Authors', 'MoreBtn'] }} />

						<div class="name-and-authors">
							{#if voki.name === 'ok'}
								<p class="voki-name">{voki.data.voki.name}</p>
								<div class="authors">
									<span class="by-label">by:</span>
									<BasicUserDisplay
										userId={voki.data.voki.primaryAuthorId}
										interactionLevel="WholeComponentLink"
									/>
									<div class="co-authors">
										{#each voki.data.voki.coAuthorIds as coAuthorId}
											<BasicUserDisplay userId={coAuthorId} interactionLevel="WholeComponentLink" />
										{/each}
									</div>
								</div>
							{/if}
						</div>

						<div class="times-taken">
							<span class="times-count">{voki.taken.timesTaken}</span>
							<span class="times-label">
								{voki.taken.timesTaken === 1 ? 'time taken' : 'times taken'}
							</span>
						</div>

						<a href="/catalog/{voki.vokiId}" class="open-voki-page">
							Open Vokis page
						</a>
						<label class="date">
							Last time taken: <span use:relativeTime={{ date: voki.taken.lastTimeTaken }} />
						</label>
					</div>
				{/each}
			</div>
		{/if}
	{/if}
{/if}

<style>
	.vokis-container {
		width: 100%;
		display: flex;
		flex-direction: column;
		gap: 0.5rem;
	}

	.voki-item {
		width: 100%;
		display: grid;
		grid-template-columns: 20rem 1fr auto auto;
		box-shadow: var(--shadow-xs);
		border-radius: calc(var(--voki-cover-border-radius) * 1.25);
		position: relative;
		background-color: var(--back);
	}

	.voki-name {
		margin: 0.5rem;
		font-size: 1.25rem;
		font-weight: 500;
		text-indent: 0.125em;
		word-break: normal;
		overflow-wrap: anywhere;
		color: var(--text);
	}

	.name-and-authors {
		display: flex;
		flex-direction: column;
		padding-right: 1rem;
	}

	.by-label {
		margin-right: 0.5rem;
		color: var(--secondary-foreground);
		font-size: 1rem;
		font-weight: 450;
	}

	.authors {
		display: flex;
		flex-direction: row;
		align-items: center;
	}

	.co-authors {
		margin: 0 0.25rem;
		color: var(--secondary-foreground);
		font-size: 1rem;
		font-weight: 450;
		display: flex;
		flex-wrap: wrap;
		gap: 0.25rem;
	}

	.times-taken {
		align-self: center;
		justify-self: center;
		display: flex;
		flex-direction: column;
		align-items: center;
		padding: 0.5rem 1rem;
	}

	.times-count {
		font-size: 1.5rem;
		font-weight: 600;
		color: var(--primary);
	}

	.times-label {
		font-size: 0.875rem;
		color: var(--secondary-foreground);
	}

	.date {
		color: var(--secondary-foreground);
		font-size: 1rem;
		font-weight: 450;
		position: absolute;
		bottom: 1rem;
		right: 1rem;
	}

	.open-voki-page {
		margin: 0 2rem;
		align-self: center;
		justify-self: center;
		font-size: 0.95rem;
		color: var(--primary);
		text-decoration: none;
	}
</style>
