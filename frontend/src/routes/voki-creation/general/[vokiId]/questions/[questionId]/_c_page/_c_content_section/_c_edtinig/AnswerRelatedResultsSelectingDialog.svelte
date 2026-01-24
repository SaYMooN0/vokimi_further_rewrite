<script lang="ts">
	import DialogWithCloseButton from '$lib/components/dialogs/DialogWithCloseButton.svelte';
	import DefaultCheckBox from '$lib/components/inputs/DefaultCheckBox.svelte';
	import PrimaryButton from '$lib/components/buttons/PrimaryButton.svelte';
	import type { Err } from '$lib/ts/err';
	import type { QuestionPageResultsState } from '../../../general-voki-creation-specific-question-page-state.svelte';
	import CubesLoader from '$lib/components/loaders/CubesLoader.svelte';
	import DefaultErrBlock from '$lib/components/errs/DefaultErrBlock.svelte';
	import { watch } from 'runed';

	interface Props {
		allResults: QuestionPageResultsState;
		fetchResultNames: () => void;
	}
	let { allResults, fetchResultNames }: Props = $props();
	let dialog = $state<DialogWithCloseButton>()!;
	let errs: Err[] = $state([]);
	let currentResultSelectedIds: string[] = $state([]);
	let resultsWithIsSelected = $state<Record<string, boolean>>({}); // id - isSelected
	watch(
		() => currentResultSelectedIds,
		() => {
			const selectedMap: Record<string, boolean> = {};
			if (allResults.state === 'ok') {
				for (const resultId of Object.keys(allResults.resultsIdToName)) {
					selectedMap[resultId] = currentResultSelectedIds.includes(resultId);
				}
			}
			resultsWithIsSelected = selectedMap;
		}
	);
	let onSubmit: () => void = $state(() => {});
	export function open(selectedResultIds: string[], setSelected: (selected: string[]) => void) {
		errs = [];
		currentResultSelectedIds = selectedResultIds;
		dialog.open();
		onSubmit = () => {
			setSelected(Object.keys(resultsWithIsSelected).filter((id) => resultsWithIsSelected[id]));
		};
	}
</script>

<DialogWithCloseButton
	dialogId="general-voki-answer-related-results-selecting-dialog"
	bind:this={dialog}
	subheading="Select related results"
>
	{#if allResults.state === 'loading'}
		<div class="loader">
			<CubesLoader sizeRem={5} color="var(--primary)" />
			<p>Loading...</p>
		</div>
	{:else if allResults.state === 'error'}
		<div class="error">
			<h1>Error during fetching results</h1>
			<DefaultErrBlock errList={allResults.errs} />
		</div>
		<PrimaryButton onclick={() => fetchResultNames()} class="refetch">Refetch</PrimaryButton>
	{:else if Object.keys(allResults.resultsIdToName).length === 0}
		<div>This Voki has no results</div>
		<div>Please save changes and go to results page to create new results</div>
		<PrimaryButton onclick={() => fetchResultNames()} class="refetch">Refetch</PrimaryButton>
	{:else}
		<div class="results">
			{#each Object.entries(allResults.resultsIdToName) as [resultId, resultName]}
				<label class="result">
					<DefaultCheckBox bind:checked={resultsWithIsSelected[resultId]} />
					<p>{resultName}</p>
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
