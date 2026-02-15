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
	<div class="results-part-container">
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
	</div>
	<div class="sep" />
	<div class="content-wrapper">
		<span class="answer-order">Answer #{order}</span>
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
	.results-part-container {
		display: flex;
		align-items: center;
		justify-content: center;
		height: 100%;
		width: 100%;
		min-height: 3rem;
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
		text-align: center;
		text-decoration: underline;
		text-decoration-thickness: 0.125rem;
	}

	.content-wrapper {
		display: flex;
		flex-direction: column;
		gap: 0.5rem;
		width: 100%;
		height: 100%;
	}

	.answer-order {
		color: var(--muted-foreground);
		font-size: 0.9rem;
		font-weight: 500;
	}
</style>
