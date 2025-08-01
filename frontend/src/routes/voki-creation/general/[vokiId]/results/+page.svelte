<script lang="ts">
	import PrimaryButton from '$lib/components/PrimaryButton.svelte';
	import ListEmptyMessage from '../../../c_shared/ListEmptyMessage.svelte';
	import UnableToLoad from '../../../c_shared/UnableToLoad.svelte';
	import VokiCreationBasicHeader from '../../../c_shared/VokiCreationBasicHeader.svelte';
	import type { PageProps } from './$types';
	import ResultInitializingDialog from './c_results_page/ResultInitializingDialog.svelte';

	let { data }: PageProps = $props();
	let resultCreationDialog = $state<ResultInitializingDialog>()!;
	let results = $state(data.data?.results ?? []);
	const maxResultsCount = 60;
</script>

{#if !data.isSuccess}
	<UnableToLoad errs={data.errs} />
{:else}
	<ResultInitializingDialog
		bind:this={resultCreationDialog}
		vokiId={data.vokiId!}
		updateParentResults={(newResults) => (results = newResults)}
	/>
	<VokiCreationBasicHeader header={`Voki results (${results.length})`} />

	{#if results.length === 0}
		<ListEmptyMessage
			messageText="This voki doesn't have any results yet"
			btnText="Create first result"
			onBtnClick={() => resultCreationDialog.open()}
		/>
	{:else}
		<div class="results">
			{#each results as result}
				<p>result</p>
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
</style>
