<script lang="ts">
	import FieldNotSetLabel from '../../../../../../../c_shared/FieldNotSetLabel.svelte';

	let {
		relatedResultIds,
		openRelatedResultsSelectingDialog
	}: {
		relatedResultIds: string[];
		openRelatedResultsSelectingDialog: () => void;
	} = $props<{
		relatedResultIds: string[];
		openRelatedResultsSelectingDialog: () => void;
	}>();
	const maxResultsCount = 10;
</script>

<div class="related-results">
	{#if relatedResultIds.length === 0}
		<FieldNotSetLabel text="related results" className="no-related-results" />
	{:else}
		<label class="related-results-label">Related relatedResultIds ({relatedResultIds.length})</label>
		{#each relatedResultIds as result}
			<div class="result">
				<label>
					{result}
				</label>
				<svg class="remove-result-btn"><use href="#common-minus-icon" /></svg>
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
		width: 100%;
		display: flex;
		flex-direction: column;
		align-items: center;
		gap: 0.375rem;
		height: 100%;
		justify-content: center;
	}
	.related-results > :global(.no-related-results) {
		margin: 0;
		background-color: var(--muted);
		color: var(--muted-foreground);
		font-weight: 450;
	}
	.related-results-label {
		color: var(--secondary-foreground);
		font-size: 1.125rem;
		font-weight: 450;
		text-decoration: underline;
		text-decoration-thickness: 0.125rem;
		margin-bottom: 0.25rem;
	}
	.result {
		display: grid;
		grid-template-columns: 1fr auto;
		width:100%;
		gap: 0.25rem;
		padding: 0 0.25rem;
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
		stroke-width: 3;
		cursor: pointer;
		background-color: var(--muted);
		color: var(--muted-foreground);
		border-radius: 0.25rem;
		padding: 0.125rem;
	}
	.remove-result-btn:hover {
		background-color: var(--accent);
		color: var(--accent-foreground);
	}
	.add-btn {
		margin-top: 0.25rem;
		display: flex;
		justify-content: center;
		align-items: center;
		gap: 0.5rem;
		background-color: var(--primary);
		border: none;
		padding: 0.125rem 1rem;
		border-radius: 4rem;
		color: var(--primary-foreground);
		font-size: 1.125rem;
		font-weight: 400;
		cursor: pointer;
		width: fit-content;
		align-self: center;
	}
	.add-btn > svg {
		height: 1.25rem;
		width: 1.25rem;
		stroke-width: 2;
	}
</style>
