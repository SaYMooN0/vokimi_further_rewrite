<script lang="ts">
	import ManageVokiPageLoadingErr from '../../../_c_shared/ManageVokiPageLoadingErr.svelte';
	import type { PageProps } from './$types';
	import NonEmptyRatingsTabContent from './_c_page/NonEmptyRatingsTabContent.svelte';
	import NoRatingsMessage from './_c_page/NoRatingsMessage.svelte';
	import type { RatingValueToCountType, ApiDistributionPresentation } from './types';

	let { data }: PageProps = $props();
	function parseResponseRating(distribution: ApiDistributionPresentation): RatingValueToCountType {
		return {
			1: distribution.rating1Count,
			2: distribution.rating2Count,
			3: distribution.rating3Count,
			4: distribution.rating4Count,
			5: distribution.rating5Count
		};
	}
	function ratingsCountNotZero(ratingToCount: RatingValueToCountType) {
		return (
			ratingToCount[1] > 0 ||
			ratingToCount[2] > 0 ||
			ratingToCount[3] > 0 ||
			ratingToCount[4] > 0 ||
			ratingToCount[5] > 0
		);
	}
</script>

{#if !data.response.isSuccess}
	<ManageVokiPageLoadingErr vokiId={data.vokiId} errs={data.response.errs} />
{:else if ratingsCountNotZero(parseResponseRating(data.response.data.distribution))}
	<NonEmptyRatingsTabContent distribution={parseResponseRating(data.response.data.distribution)} />
{:else}
	<NoRatingsMessage vokiId={data.vokiId} />
{/if}
