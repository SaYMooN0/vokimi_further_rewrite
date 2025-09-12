<script lang="ts">
	import { goto } from '$app/navigation';
	import DialogWithCloseButton from '$lib/components/dialogs/DialogWithCloseButton.svelte';
	import DefaultErrBlock from '$lib/components/errs/DefaultErrBlock.svelte';
	import DefaultCheckBox from '$lib/components/inputs/DefaultCheckBox.svelte';
	import CubesLoader from '$lib/components/loaders/CubesLoader.svelte';
	import PrimaryButton from '$lib/components/buttons/PrimaryButton.svelte';
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
	subheading="Select related results"
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
		<PrimaryButton onclick={() => fetchResultNames()} class="refetch">Refetch</PrimaryButton>
	{:else if allResults.length === 0}
		<ListEmptyMessage
			messageText="This voki has no results"
			btnText="Go to results page"
			onBtnClick={() => {
				goto(`/voki-creation/general/${vokiId}/results`);
			}}
			className="no-results"
		/>
		<PrimaryButton onclick={() => fetchResultNames()} class="refetch">Refetch</PrimaryButton>
	{:else}
		<div class="results">
			{#each allResults as result}
				<label class="result">
					<DefaultCheckBox bind:checked={resultsWithIsSelected[result.id]} />
					<p>{result.name}</p>
				</label>
			{/each}
		</div>
		<PrimaryButton onclick={() => onSubmit()} class="confirm">Confirm</PrimaryButton>
	{/if}
</DialogWithCloseButton>

<style>
	:global(#general-voki-answer-related-results-selecting-dialog .dialog-content) {
		min-width: 40rem;
		min-height: 20rem;
	}

	.loader {
		display: flex;
		flex-direction: column;
		justify-content: center;
		align-items: center;
		gap: 1rem;
		width: 100%;
		height: 100%;
		animation: var(--default-fade-in-animation);
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
		animation: var(--default-fade-in-animation);
	}

	:global(#general-voki-answer-related-results-selecting-dialog .refetch) {
		margin-top: 4rem;
	}

	:global(#general-voki-answer-related-results-selecting-dialog .no-results) {
		margin: auto;
		animation: var(--default-fade-in-animation);
	}

	.results {
		display: flex;
		flex-direction: column;
		width: 100%;
		max-height: 50rem;
		overflow-y: auto;
		padding: 0.25rem 1rem;
	}

	.result {
		display: flex;
		place-items: center flex-start;
		gap: 0.5rem;
		width: 100%;
		padding: 0.25rem 0.5rem;
		border-radius: 0.5rem;
		font-size: 1.25rem;
		font-weight: 450;
	}

	.result:hover {
		background-color: var(--secondary);
		box-shadow: var(--shadow-xs);
		cursor: pointer;
	}

	:global(#general-voki-answer-related-results-selecting-dialog .confirm) {
		margin-top: auto;
	}

	@keyframes default-fade-in {
		from {
			opacity: 0.4;
		}

		to {
			opacity: 1;
		}
	}
</style>
