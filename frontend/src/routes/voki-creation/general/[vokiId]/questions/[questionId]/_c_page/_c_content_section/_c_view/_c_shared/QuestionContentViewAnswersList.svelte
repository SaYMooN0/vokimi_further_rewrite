<script lang="ts" generics="T extends BaseGeneralVokiAnswerData">
	import type { Snippet } from 'svelte';
	import type { BaseGeneralVokiAnswerData } from '../../../../types';
	import FieldNotSetLabel from '$lib/components/FieldNotSetLabel.svelte';
	import SingleAnswerWrapper from '../../_c_shared/SingleAnswerWrapper.svelte';

	interface Props {
		answers: T[];
		answerContentSnippet: Snippet<[T]>;
		resultsIdToName: Record<string, string>;
	}
	let { answers, answerContentSnippet, resultsIdToName }: Props = $props();
</script>

{#if answers.length === 0}
	<FieldNotSetLabel text="answers" />
{:else}
	<div class="answer-list">
		{#each answers as answer}
			<SingleAnswerWrapper>
				{#snippet results()}
					<div class="related-results">
						<label class="related-results-label"
							>Related results ({answer.relatedResultIds.length})</label
						>
						{#if answer.relatedResultIds.length === 0}
							<FieldNotSetLabel text="related results" class="no-related-results" />
						{:else}
							{#each answer.relatedResultIds as result}
								<div class="result" class:err={!resultsIdToName[result]}>
									{resultsIdToName[result] ?? 'error'}
								</div>
							{/each}
						{/if}
					</div>
				{/snippet}
				{#snippet answerContent()}
					{@render answerContentSnippet(answer)}
				{/snippet}
			</SingleAnswerWrapper>
		{/each}
	</div>
{/if}

<style>
	.answer-list {
		display: flex;
		flex-direction: column;
		place-items: center center;
		width: 100%;
	}
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
		background-color: var(--red-1);
		color: var(--err-foreground);
		box-shadow: var(--err-shadow);
	}
</style>
