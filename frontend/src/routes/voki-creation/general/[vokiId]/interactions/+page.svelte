<script lang="ts">
	import InteractionsPageSharedComponent from '../../../c_shared/c_shared_pages/InteractionsPageSharedComponent.svelte';
	import UnableToLoad from '../../../c_shared/UnableToLoad.svelte';
	import type { PageProps } from './$types';

	let { data }: PageProps = $props();
</script>

{#if !data.response.isSuccess}
	<UnableToLoad errs={data.response.errs} />
{:else}
	<InteractionsPageSharedComponent />
	resultsVisibility : GeneralVokiResultsVisibility; showResultsDistribution : boolean;
{/if}
