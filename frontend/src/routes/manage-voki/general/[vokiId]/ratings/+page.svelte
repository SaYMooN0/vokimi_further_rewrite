<script lang="ts">
	import ManageVokiPageLoadingErr from '../../../_c_shared/ManageVokiPageLoadingErr.svelte';
	import type { PageProps } from './$types';
	import NonEmptyRatingsTabContent from './_c_page/NonEmptyRatingsTabContent.svelte';
	import NoRatingsMessage from './_c_page/NoRatingsMessage.svelte';
	import type { RatingValueToCountType } from './types';

	let { data }: PageProps = $props();
	function ratingsCountNotZero(ratings: RatingValueToCountType) {
		return ratings[1] > 0 || ratings[2] > 0 || ratings[3] > 0 || ratings[4] > 0 || ratings[5] > 0;
	}
</script>

{#if !data.response.isSuccess}
	<ManageVokiPageLoadingErr vokiId={data.vokiId} errs={data.response.errs} />
{:else if ratingsCountNotZero(data.response.data.distribution)}
	<NonEmptyRatingsTabContent distribution={data.response.data.distribution} />
{:else}
	<NoRatingsMessage vokiId={data.vokiId} />
{/if}
