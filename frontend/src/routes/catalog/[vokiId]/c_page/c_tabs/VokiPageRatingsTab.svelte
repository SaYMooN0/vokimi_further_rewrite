<script lang="ts">
	import type { VokiRatingData } from '../../types';
	import RatingsTabAverageRating from './c_ratings_tab/RatingsTabAverageRating.svelte';
	import RatingsTabOtherRatingsList from './c_ratings_tab/RatingsTabOtherRatingsList.svelte';
	import RatingsTabUserRating from './c_ratings_tab/RatingsTabUserRating.svelte';

	type userRatingData =
		| { hasTaken: false }
		| { hasTaken: true; rating: null }
		| { hasTaken: true; rating: { value: number; lastUpdated: Date } };

	interface Props {
		userRatingData: userRatingData;
		otherRatings: VokiRatingData[];
		averageRating: number;
	}
	let { userRatingData, otherRatings, averageRating }: Props = $props();
</script>

<div class="ratings-page-container">
	<header class="avg-wrap">
		<h1 class="title">Ratings</h1>
		<RatingsTabAverageRating {averageRating} count={0} />
	</header>

	<RatingsTabUserRating />

	<RatingsTabOtherRatingsList />
</div>
