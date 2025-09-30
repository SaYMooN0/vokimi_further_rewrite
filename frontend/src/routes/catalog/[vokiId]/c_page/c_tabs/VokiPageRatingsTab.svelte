<script lang="ts">
	import AuthView from '$lib/components/AuthView.svelte';
	import ReloadButton from '$lib/components/buttons/ReloadButton.svelte';
	import DefaultErrBlock from '$lib/components/errs/DefaultErrBlock.svelte';
	import CubesLoader from '$lib/components/loaders/CubesLoader.svelte';
	import type { ResponseResult } from '$lib/ts/backend-communication/result-types';
	import type { Err } from '$lib/ts/err';
	import type { RatingsTabDataType } from '../../types';
	import UserRatingAuthNeeded from './c_ratings_tab/c_user_rating/UserRatingAuthNeeded.svelte';
	import UserRatingVokiTakingNeeded from './c_ratings_tab/c_user_rating/UserRatingVokiTakingNeeded.svelte';
	import RatingsTabAverageRating from './c_ratings_tab/RatingsTabAverageRating.svelte';
	import RatingsTabOtherRatingsList from './c_ratings_tab/RatingsTabOtherRatingsList.svelte';
	import RatingsTabUserRating from './c_ratings_tab/RatingsTabUserRating.svelte';

	interface Props {
		tabData: RatingsTabDataType;
		fetchTabData: () => Promise<void>;
		saveNewUserRating: (
			value: number
		) => Promise<ResponseResult<{ value: number; dateTime: Date }>>;
	}
	let { tabData, fetchTabData, saveNewUserRating }: Props = $props();

	if (tabData.state == 'empty') {
		fetchTabData();
	}
</script>

<div class="ratings-tab-container">
	{#if tabData.state === 'loading'}
		<div class="loading-container">
			<CubesLoader sizeRem={5} />
			<h1 class="loading-text">Loading Voki ratings</h1>
		</div>
	{:else if tabData.state === 'error'}
		<h1 class="error-text">Ratings data loading error</h1>
		<DefaultErrBlock errList={tabData.errs} />
		<ReloadButton onclick={() => fetchTabData()} />
	{:else if tabData.state === 'empty'}
		<h1 class="error-text">Something went wrong</h1>
		<ReloadButton onclick={() => fetchTabData()} />
	{:else if tabData.state === 'fetched'}
		<RatingsTabAverageRating
			averageRating={tabData.averageRating}
			count={tabData.allRatings.length}
		/>

		<AuthView>
			{#snippet authenticated(authData)}
				{#if tabData.userHasTaken}
					<RatingsTabUserRating
						{saveNewUserRating}
						userRating={tabData.allRatings.find((r) => r.userId === authData.userId)}
					/>
				{:else}
					<UserRatingVokiTakingNeeded />
				{/if}
			{/snippet}
			{#snippet unauthenticated()}
				<UserRatingAuthNeeded />
			{/snippet}
		</AuthView>

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

	.loading-container {
		display: flex;
		flex-direction: column;
		align-items: center;
		gap: 1rem;
		margin: 2rem 0;
	}

	.loading-container > h1 {
		color: var(--secondary-foreground);
		font-size: 1.75rem;
		font-weight: 550;
		letter-spacing: 0.25px;
	}
</style>
