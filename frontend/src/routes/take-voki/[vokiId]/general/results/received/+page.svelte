<script lang="ts">
	import PageLoadErrView from '$lib/components/PageLoadErrView.svelte';
	import GeneralVokiResultPagesHeading from '../c_pages_shared/GeneralVokiResultPagesHeading.svelte';
	import GeneralVokiResultPagesVokiNameSpan from '../c_pages_shared/GeneralVokiResultPagesVokiNameSpan.svelte';
	import type { PageProps } from './$types';

	let { data }: PageProps = $props();
</script>

{#if data.response.isSuccess}
	<GeneralVokiResultPagesHeading>
		My received results of the
		<GeneralVokiResultPagesVokiNameSpan vokiName={data.response.data.vokiName} />
		general Voki ({data.response.data.results.length} out of {data.response.data.resultsCount})
	</GeneralVokiResultPagesHeading>
{:else}
	<PageLoadErrView
		defaultMessage="Unable to load all received voki results "
		errs={data.response.errs}
		additionalParams={[{ name: 'vokiId', value: data.vokiId }]}
	/>
{/if}
