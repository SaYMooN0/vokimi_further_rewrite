<script lang="ts">
	import DefaultErrBlock from '$lib/components/errs/DefaultErrBlock.svelte';
	import CubesLoader from '$lib/components/loaders/CubesLoader.svelte';
	import type { RatingsTabDataType } from '../../types';
	import RatingsTabAverageRating from './c_ratings_tab/RatingsTabAverageRating.svelte';
	import RatingsTabOtherRatingsList from './c_ratings_tab/RatingsTabOtherRatingsList.svelte';
	import RatingsTabUserRating from './c_ratings_tab/RatingsTabUserRating.svelte';

	interface Props {
		tabData: RatingsTabDataType;
		fetchTabData: () => Promise<void>;
	}
	let { tabData, fetchTabData }: Props = $props();

	// if (tabData.state == 'empty') {
	// 	fetchTabData();
	// }
</script>

<div class="ratings-tab-container">
	{#if tabData.state === 'loading'}
		<CubesLoader sizeRem={5} />
		<h1 class="loading-text">Loading Voki ratings</h1>
	{:else if tabData.state === 'error'}
		<h1 class="error-text">Ratings data loading error</h1>
		<DefaultErrBlock errList={tabData.errs} />
	{:else if tabData.state === 'empty'}
		{#await fetchTabData()}{/await}
	{:else if tabData.state === 'fetched'}
		<!-- <RatingsTabAverageRating  averageRating={tabData.state} count={0} /> -->
		<RatingsTabUserRating />

		<RatingsTabOtherRatingsList />
	{:else}
		<h1>Something is wrong</h1>
	{/if}
</div>

<style>
	.ratings-tab-container {
		display: flex;
		flex-direction: column;
		gap: 0.5rem;
	}
	.loading-text {
		font-size: 1.5rem;
		color: var(--secondary-foreground);
		font-weight: 500;
		letter-spacing: 0.25px;
	}
</style>
