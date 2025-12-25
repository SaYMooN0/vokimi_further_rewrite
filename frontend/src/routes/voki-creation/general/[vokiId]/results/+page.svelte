<script lang="ts">
	import PrimaryButton from '$lib/components/buttons/PrimaryButton.svelte';
	import ListEmptyMessage from '../../../_c_shared/ListEmptyMessage.svelte';
	import VokiCreationBasicHeader from '../../../_c_shared/VokiCreationBasicHeader.svelte';
	import type { PageProps } from './$types';
	import GeneralVokiCreationResultItem from './_c_results_page/GeneralVokiCreationResultItem.svelte';
	import ResultInitializingDialog from './_c_results_page/ResultInitializingDialog.svelte';
	import type { ResultOverViewData } from './types';
	import VokiCreationPageLoadingErr from '../../../_c_shared/VokiCreationPageLoadingErr.svelte';

	let { data }: PageProps = $props();
	let resultCreationDialog = $state<ResultInitializingDialog>()!;
	let results = $state(data.data?.results ?? []);

	function updateOnSave(result: ResultOverViewData) {
		results = results.map((r) => (r.id === result.id ? result : r));
	}
	function updateOnDelete(resultId: string) {
		results = results.filter((r) => r.id !== resultId);
	}
	const maxResultsCount = 60;
</script>

{#if !data.isSuccess}
	<VokiCreationPageLoadingErr vokiId={data.vokiId!} errs={data.errs} />
{:else}
	<ResultInitializingDialog
		bind:this={resultCreationDialog}
		vokiId={data.vokiId!}
		updateParentResults={(newResults) => (results = newResults)}
	/>

	{#if results.length === 0}
		<ListEmptyMessage
			messageText="This voki doesn't have any results yet"
			btnText="Create first result"
			onBtnClick={() => resultCreationDialog.open()}
		/>
	{:else}
		<VokiCreationBasicHeader header={`Voki results (${results.length})`} />
		<div class="results">
			{#each results as result}
				<GeneralVokiCreationResultItem
					vokiId={data.vokiId!}
					{result}
					updateParentOnSave={updateOnSave}
					updateParentOnDelete={updateOnDelete}
				/>
			{/each}
		</div>
		{#if results.length < maxResultsCount}
			<div class="add-new-result-btn-container">
				<PrimaryButton onclick={() => resultCreationDialog.open()}>Add new result</PrimaryButton>
			</div>
		{/if}
	{/if}
{/if}

<style>
	.add-new-result-btn-container {
		display: flex;
		justify-content: center;
		margin: 1.25rem auto;
	}

	.results {
		display: flex;
		flex-direction: column;
		gap: 1.25rem;
	}
</style>
