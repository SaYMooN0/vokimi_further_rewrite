<script lang="ts">
	import FieldNotSetLabel from '../../../../../../../../../c_shared/FieldNotSetLabel.svelte';
	import { getQuestionPageContext } from '../../../../../question-page-context.svelte';

	let {
		relatedResultIds,
		openRelatedResultsSelectingDialog,
		removeResult
	}: {
		relatedResultIds: string[];
		openRelatedResultsSelectingDialog: () => void;
		removeResult: (resultId: string) => void;
	} = $props<{
		relatedResultIds: string[];
		openRelatedResultsSelectingDialog: () => void;
		removeResult: (resultId: string) => void;
	}>();
	const maxResultsCount = 10;
	let resultsIdToName = Object.fromEntries(
		getQuestionPageContext().results.map((r) => [r.id, r.name])
	);
</script>

<div class="related-results">
	{#if relatedResultIds.length === 0}
		<FieldNotSetLabel text="related results" className="no-related-results" />
	{:else}
		<label class="related-results-label">Related results ({relatedResultIds.length})</label>
		{#each relatedResultIds as id}
			<div class="result" class:err={resultsIdToName[id] === undefined}>
				{#if resultsIdToName[id]}
					<label>
						{resultsIdToName[id]}
					</label>
					<svg class="remove-result-btn" onclick={() => removeResult(id)}
						><use href="#common-minus-icon" /></svg
					>
				{:else}
					<label>Error</label>
				{/if}
			</div>
		{/each}
	{/if}
	{#if relatedResultIds.length < maxResultsCount}
		<button class="add-btn" onclick={() => openRelatedResultsSelectingDialog()}>
			<svg><use href="#common-plus-icon" /></svg>
			related results
		</button>
	{/if}
</div>

<style>
	.related-results {
		display: flex;
		flex-direction: column;
		justify-content: center;
		align-items: center;
		gap: 0.375rem;
		width: 100%;
		height: 100%;
	}

	.related-results > :global(.no-related-results) {
		margin: 0;
		font-weight: 450;
	}

	.related-results-label {
		margin-bottom: 0.25rem;
		color: var(--secondary-foreground);
		font-size: 1.125rem;
		font-weight: 450;
		text-decoration: underline;
		text-decoration-thickness: 0.125rem;
	}

	.result {
		display: grid;
		gap: 0.25rem;
		width: 100%;
		padding: 0 0.25rem;
		grid-template-columns: 1fr auto;
	}

	.result.err {
		padding: 0.125rem;
		border-radius: 0.25rem;
		background-color: var(--err-back);
		color: var(--err-foreground);
		box-shadow: var(--err-shadow);
	}

	.result > label {
		text-overflow: ellipsis;
		overflow: hidden;
		white-space: nowrap;
		color: var(--text);
		font-size: 1.25rem;
		font-weight: 450;
	}

	.remove-result-btn {
		width: 1.375rem;
		height: 1.375rem;
		padding: 0.125rem;
		border-radius: 0.25rem;
		background-color: var(--muted);
		color: var(--muted-foreground);
		cursor: pointer;
		stroke-width: 3;
	}

	.remove-result-btn:hover {
		background-color: var(--accent);
		color: var(--accent-foreground);
	}

	.add-btn {
		display: flex;
		justify-content: center;
		align-items: center;
		gap: 0.5rem;
		width: fit-content;
		padding: 0.125rem 1rem;
		margin-top: 0.25rem;
		border: none;
		border-radius: 4rem;
		background-color: var(--primary);
		color: var(--primary-foreground);
		font-size: 1.125rem;
		font-weight: 400;
		cursor: pointer;
		align-self: center;
	}

	.add-btn > svg {
		width: 1.25rem;
		height: 1.25rem;
		stroke-width: 2;
	}
</style>
