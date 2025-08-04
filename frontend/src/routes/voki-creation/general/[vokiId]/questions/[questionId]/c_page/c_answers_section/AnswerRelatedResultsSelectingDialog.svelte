<script lang="ts">
	import { goto } from '$app/navigation';
	import DialogWithCloseButton from '$lib/components/dialogs/DialogWithCloseButton.svelte';
	import DefaultErrBlock from '$lib/components/errs/DefaultErrBlock.svelte';
	import CubesLoader from '$lib/components/loaders/CubesLoader.svelte';
	import PrimaryButton from '$lib/components/PrimaryButton.svelte';
	import { ApiVokiCreationGeneral } from '$lib/ts/backend-communication/voki-creation-backend-service';
	import type { Err } from '$lib/ts/err';
	import ListEmptyMessage from '../../../../../../c_shared/ListEmptyMessage.svelte';
	import { getQuestionPageContext } from '../../question-page-context.svelte';
	import type { ResultIdWithName } from '../../types';

	const { vokiId }: { vokiId: string } = $props<{ vokiId: string }>();
	let dialog = $state<DialogWithCloseButton>()!;
	let errs: Err[] = $state([]);
	let allResults = $state<ResultIdWithName[]>(getQuestionPageContext().results);
	let isLoading = $state(false);
	let resultsWithIsSelected = $state<Record<string, boolean>>({}); // id - isSelected
	let onSubmit: () => void = $state(() => {});
	export function open(selectedResultIds: string[], setSelected: (selected: string[]) => void) {
		errs = [];
		const selectedMap: Record<string, boolean> = {};
		for (const result of allResults) {
			selectedMap[result.id] = selectedResultIds.includes(result.id);
		}
		resultsWithIsSelected = selectedMap;
		dialog.open();
		onSubmit = () => {
			setSelected(Object.keys(resultsWithIsSelected).filter((id) => resultsWithIsSelected[id]));
		};
	}

	async function fetchResultNames() {
		isLoading = true;
		const response = await ApiVokiCreationGeneral.fetchJsonResponse<{
			results: ResultIdWithName[];
		}>(`/vokis/${vokiId}/results/ids-names`, { method: 'GET' });

		if (response.isSuccess) {
			allResults = response.data.results;
		} else {
			errs = response.errs;
		}
		isLoading = false;
	}
</script>

<DialogWithCloseButton
	dialogId="general-voki-answer-related-results-selecting-dialog"
	bind:this={dialog}
>
	{#if isLoading}
		<div class="loader">
			<CubesLoader sizeRem={5} />
			<p>Loading...</p>
		</div>
	{:else if errs.length > 0}
		<div class="error">
			<h1>Error during fetching results</h1>
			<DefaultErrBlock errList={errs} />
		</div>
		<PrimaryButton onclick={() => fetchResultNames()}>Refetch</PrimaryButton>
	{:else if allResults.length === 0}
		<ListEmptyMessage
			messageText="This voki has no results"
			btnText="Go to results page"
			onBtnClick={() => {
				goto(`/voki-creation/general/${vokiId}/results`);
			}}
			className="no-results"
		/>
		<PrimaryButton onclick={() => fetchResultNames()}>Refetch</PrimaryButton>
	{:else}
		<div class="results">
			{#each allResults as result}
				<div class="result">
					<input type="checkbox" bind:checked={resultsWithIsSelected[result.id]} />
					<p>{result.name}</p>
				</div>
			{/each}
		</div>
		<button class="submit-btn" onclick={onSubmit}>Confirm</button>
	{/if}
</DialogWithCloseButton>

<style>
	:global(#general-voki-answer-related-results-selecting-dialog .dialog-content) {
		width: 50rem;
		height: 30rem;
	}

	.loader {
		display: flex;
		flex-direction: column;
		justify-content: center;
		align-items: center;
		gap: 1rem;
		width: 100%;
		height: 100%;
	}

	.loader p {
		color: var(--secondary-foreground);
		font-size: 2rem;
		font-weight: 520;
		letter-spacing: 1.5px;
	}

	.error {
		display: flex;
		flex-direction: column;
		align-items: center;
		gap: 1rem;
		margin: auto;
	}

	.error > :global(.primary-btn) {
		margin-top: 1rem;
	}

	:global(#general-voki-answer-related-results-selecting-dialog .no-results) {
		margin: auto;
	}
</style>
