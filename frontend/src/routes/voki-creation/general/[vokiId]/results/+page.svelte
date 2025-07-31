<script lang="ts">
	import ListEmptyMessage from '../../../c_shared/ListEmptyMessage.svelte';
	import UnableToLoad from '../../../c_shared/UnableToLoad.svelte';
	import type { PageProps } from './$types';
	import ResultInitializingDialog from './c_results_page/ResultInitializingDialog.svelte';

	let { data }: PageProps = $props();
	let questionInitializingDialog = $state<ResultInitializingDialog>()!;
	const maxResultsCount = 60;
</script>

{#if !data.isSuccess}
	<UnableToLoad errs={data.errs} />
{:else}
	<ResultInitializingDialog bind:this={questionInitializingDialog} vokiId={data.vokiId!} />
	{#if data.data.results.length === 0}
		<ListEmptyMessage
			messageText="This voki doesn't have any results yet"
			btnText="Create first result"
			onBtnClick={() => questionInitializingDialog.open()}
		/>
	{:else}
		<div class="results">
			{#each data.data.results as result}
				<p>result</p>
			{/each}
		</div>
	{/if}
{/if}
