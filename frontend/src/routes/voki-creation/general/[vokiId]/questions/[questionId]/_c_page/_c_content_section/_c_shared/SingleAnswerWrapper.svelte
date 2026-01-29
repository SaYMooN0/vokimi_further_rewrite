<script lang="ts">
	import type { Snippet } from 'svelte';
	import type { QuestionPageResultsState } from '../../../general-voki-creation-specific-question-page-state.svelte';

	interface Props {
		resultsViewSnippet: Snippet<[Record<string, string>]>;
		answerContentSnippet: Snippet;
		answerRelatedResultsCount: number;
		resultsIdToName: QuestionPageResultsState;
		order: number;
	}
	let {
		resultsViewSnippet,
		answerContentSnippet,
		resultsIdToName,
		answerRelatedResultsCount,
		order
	}: Props = $props();
</script>

<div class="answer">
	{#if resultsIdToName.state === 'error'}
		<div class="error">error</div>
	{:else if resultsIdToName.state === 'loading'}
		<div class="loading">loading</div>
	{:else if resultsIdToName.state === 'ok'}
		<div class="results">
			<label class="related-results-label">Related results ({answerRelatedResultsCount})</label>
			{@render resultsViewSnippet?.(resultsIdToName.resultsIdToName)}
		</div>
	{/if}
	<div class="sep" />
	<div class="content-wrapper">
		<span class="answer-order">Answer {order}</span>
		{@render answerContentSnippet?.()}
	</div>
</div>

<style>
	.sep {
		width: 1px;
		height: calc(100%);
		margin: 0 0.5rem;
		background-color: var(--muted);
		align-self: center;
	}
	.answer {
		--results-width: 13rem;

		display: grid;
		grid-template-columns: var(--results-width) auto 1fr;
		gap: 0.25rem;
		width: 100%;
		padding: 0.5rem 0.75rem;
		margin-top: 1rem;
		border-radius: 0.75rem;
		box-shadow: rgb(0 0 0 / 5%) 0 0 0 1px;
	}
	.results {
		display: flex;
		flex-direction: column;
		gap: 0.25rem;
	}
	.related-results-label {
		margin-bottom: 0.25rem;
		color: var(--secondary-foreground);
		font-size: 1.125rem;
		font-weight: 425;
		text-decoration: underline;
		text-decoration-thickness: 0.125rem;
		text-align: center;
	}
	.content-wrapper {
		display: flex;
		flex-direction: column;
		gap: 0.5rem;
		width: 100%;
	}
	.answer-order {
		font-weight: 500;
		color: var(--muted-foreground);
		font-size: 0.9rem;
	}
</style>
