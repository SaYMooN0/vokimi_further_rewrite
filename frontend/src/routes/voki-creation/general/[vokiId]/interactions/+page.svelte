<script lang="ts">
	import InteractionsPageSharedComponent from '../../../c_shared/c_shared_pages/InteractionsPageSharedComponent.svelte';
	import VokiCreationPageLoadingErr from '../../../c_shared/VokiCreationPageLoadingErr.svelte';
	import type { PageProps } from './$types';

	let { data }: PageProps = $props();
</script>

{#if !data.response.isSuccess}
	<VokiCreationPageLoadingErr vokiId={data.vokiId!} errs={data.response.errs} />
{:else}
	<InteractionsPageSharedComponent />
	resultsVisibility : GeneralVokiResultsVisibility; showResultsDistribution : boolean;
{/if}
