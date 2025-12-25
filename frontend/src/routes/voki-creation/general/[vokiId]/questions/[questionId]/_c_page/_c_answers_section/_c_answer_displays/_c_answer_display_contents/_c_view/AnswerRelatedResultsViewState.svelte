<script lang="ts">
	import FieldNotSetLabel from '../../../../../../../../../../../lib/components/FieldNotSetLabel.svelte';
	import { getQuestionPageContext } from '../../../../../question-page-context.svelte';

	let resultsIdToName = Object.fromEntries(
		getQuestionPageContext().results.map((r) => [r.id, r.name])
	);
	let { relatedResultIds }: { relatedResultIds: string[] } = $props<{
		relatedResultIds: string[];
	}>();
</script>

<div class="related-results">
	<label class="related-results-label">Related results ({relatedResultIds.length})</label>
	{#if relatedResultIds.length === 0}
		<FieldNotSetLabel text="related results" class="no-related-results" />
	{:else}
		{#each relatedResultIds as result}
			<div class="result" class:err={resultsIdToName[result] === undefined}>
				{resultsIdToName[result] ?? 'error'}
			</div>
		{/each}
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
		display: block;
		width: 100%;
		text-overflow: ellipsis;
		overflow: hidden;
		white-space: nowrap;
		color: var(--text);
		font-size: 1.125rem;
		font-weight: 450;
	}

	.result.err {
		padding: 0.125rem;
		border-radius: 0.25rem;
		background-color: var(--err-back);
		color: var(--err-foreground);
		box-shadow: var(--err-shadow);
	}
</style>
