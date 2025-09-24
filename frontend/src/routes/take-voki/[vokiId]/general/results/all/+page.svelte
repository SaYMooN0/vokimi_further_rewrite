<script lang="ts">
	import PageLoadErrView from '$lib/components/PageLoadErrView.svelte';
	import GeneralVokiResultPagesHeading from '../c_pages_shared/GeneralVokiResultPagesHeading.svelte';
	import GeneralVokiResultPagesVokiNameSpan from '../c_pages_shared/GeneralVokiResultPagesVokiNameSpan.svelte';
	import type { PageProps } from './$types';
	import GeneralVokiResultWithDistributionItem from './c_page/GeneralVokiResultWithDistributionItem.svelte';

	let { data }: PageProps = $props();
</script>

{#if data.response.isSuccess}
	<GeneralVokiResultPagesHeading>
		All results of the
		<GeneralVokiResultPagesVokiNameSpan vokiName={data.response.data.vokiName} />
		general Voki
	</GeneralVokiResultPagesHeading>
	<div class="results-grid">
		{#each data.response.data.results as r}
			<div class="results-list">
				<GeneralVokiResultWithDistributionItem
					showDistribution={data.response.data.showDistribution}
					result={r}
				/>
			</div>
		{/each}
	</div>
{:else}
	<PageLoadErrView
		defaultMessage="Unable to load all voki results "
		errs={data.response.errs}
		additionalParams={[{ name: 'vokiId', value: data.vokiId }]}
	/>
{/if}

<style>
	.results-grid {
		display: flex;
		flex-direction: column;
		gap: 0.75rem;
	}
</style>
