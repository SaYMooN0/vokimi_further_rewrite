<script lang="ts">
	import DialogWithCloseButton from '$lib/components/dialogs/DialogWithCloseButton.svelte';
	import DefaultErrBlock from '$lib/components/errs/DefaultErrBlock.svelte';
	import CubesLoader from '$lib/components/loaders/CubesLoader.svelte';
	import { ApiVokiCreationGeneral } from '$lib/ts/backend-communication/voki-creation-backend-service';
	import type { Err } from '$lib/ts/err';
	import type { ResultIdWithName } from '../../../types';

	const { vokiId }: { vokiId: string } = $props<{ vokiId: string }>();
	let dialog = $state<DialogWithCloseButton>()!;
	let errs: Err[] = $state([]);
	let allResults = $state<ResultIdWithName[] | null>(null);
	let isLoading = $state(false);
	let resultsWithIsSelected = $state<Record<string, boolean>>({}); // id - isSelected
	let onSubmit: () => void = $state(() => {});
	export function open(
		selectedResultIds: string[],
		setSelected: (selected: ResultIdWithName[]) => void
	) {
		errs = [];

		function applySelectedIds() {
			const selectedMap: Record<string, boolean> = {};
			for (const result of allResults!) {
				selectedMap[result.id] = selectedResultIds.includes(result.id);
			}
			resultsWithIsSelected = selectedMap;
		}

		if (allResults === null) {
			fetchResultNames().then(() => {
				if (allResults !== null) {
					applySelectedIds();
				}
			});
		} else {
			applySelectedIds();
		}
		onSubmit = () => {
			const selected = allResults!.filter((result) => resultsWithIsSelected[result.id]);
			setSelected(selected);
		};
		dialog.open();
	}

	async function fetchResultNames() {
		isLoading = true;
		const response = await ApiVokiCreationGeneral.fetchJsonResponse<{
			results: ResultIdWithName[];
		}>(`/vokis/${vokiId}/results/names`, { method: 'GET' });
		if (response.isSuccess) {
			allResults = response.data.results;
		} else {
			allResults = null;
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
		</div>
	{:else if allResults === null}
		{#if errs.length > 0}
			<DefaultErrBlock errList={errs} />
		{:else}
			<div class="error">
				<h1>Something went wrong during fetching voki results</h1>
				<p>Please try again later</p>
			</div>
		{/if}
	{:else}
		<div class="results">
			{#each allResults as result}
				<div class="result">
					<input type="checkbox" bind:value={resultsWithIsSelected[result.id]} />
					<p>{result}</p>
				</div>
			{/each}
		</div>
		<button class="submit-btn" onclick={onSubmit}>Submit</button>
	{/if}
</DialogWithCloseButton>
