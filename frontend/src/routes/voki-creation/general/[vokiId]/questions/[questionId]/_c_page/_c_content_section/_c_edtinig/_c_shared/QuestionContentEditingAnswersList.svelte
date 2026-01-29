<script lang="ts" generics="T extends BaseGeneralVokiAnswerData">
	import QuestionContentEditingAnswerResults from './_c_answers_list/QuestionContentEditingAnswerResults.svelte';
	import type { Snippet } from 'svelte';
	import type { BaseGeneralVokiAnswerData } from '../../../../types';
	import type { QuestionPageResultsState } from '../../../../general-voki-creation-specific-question-page-state.svelte';
	import QuestionContentEditingNoAnswers from './_c_answers_list/QuestionContentEditingNoAnswers.svelte';
	import SingleAnswerWrapper from '../../_c_shared/SingleAnswerWrapper.svelte';
	import VokiCreationDefaultButton from '../../../../../../../../_c_shared/VokiCreationDefaultButton.svelte';

	interface Props {
		answers: T[];
		answerMainContent: Snippet<[T, (newAnswer: T) => void]>;
		maxAnswersForQuestionCount: number;
		addNewAnswer: () => void;
		resultsIdToNameState: QuestionPageResultsState;
		maxResultsForAnswerCount: number;
		openRelatedResultsSelectingDialog: (
			selectedResultIds: string[],
			setSelected: (selected: string[]) => void
		) => void;
	}
	let {
		answers = $bindable(),
		answerMainContent,
		maxAnswersForQuestionCount,
		addNewAnswer,
		resultsIdToNameState,
		maxResultsForAnswerCount,
		openRelatedResultsSelectingDialog
	}: Props = $props();

	function removeResultFromAnswer(answer: T, resultId: string) {
		answer.relatedResultIds = answer.relatedResultIds.filter((rId) => rId !== resultId);
	}

	function removeAnswer(answer: T) {
		answers = answers.filter((a) => a !== answer);
	}
</script>

{#if answers.length === 0}
	<QuestionContentEditingNoAnswers {addNewAnswer} />
{:else}
	<div class="answer-list">
		{#each answers as answer, answerKey}
			<SingleAnswerWrapper
				resultsIdToName={resultsIdToNameState}
				answerRelatedResultsCount={answer.relatedResultIds.length}
				order={answer.order}
			>
				{#snippet resultsViewSnippet(idToName)}
					<QuestionContentEditingAnswerResults
						answerResults={answer.relatedResultIds}
						setAnswerResults={(results) => (answer.relatedResultIds = results)}
						removeResultFromAnswer={(resId) => removeResultFromAnswer(answer, resId)}
						resultsIdToName={idToName}
						{maxResultsForAnswerCount}
						{openRelatedResultsSelectingDialog}
					/>
				{/snippet}
				{#snippet answerContentSnippet()}
					<div class="answer-content-grid">
						{@render answerMainContent(answer, (newAnswer) => {
							answers[answerKey] = newAnswer;
						})}
						<button class="delete-answer-btn" onclick={() => removeAnswer(answer)}>
							<svg><use href="#common-minus-icon" /></svg>
						</button>
					</div>
				{/snippet}
			</SingleAnswerWrapper>
		{/each}
		{#if answers.length < maxAnswersForQuestionCount}
			<VokiCreationDefaultButton
				class="add-new-answer-btn"
				text="Add new answer"
				onclick={addNewAnswer}
			/>
		{:else}
			Answers count limit for question reached ({answers.length}/{maxAnswersForQuestionCount})
		{/if}
	</div>
{/if}

<style>
	.answer-content-grid {
		display: grid;
		grid-template-columns: 1fr auto;
		gap: 0.5rem;
		width: 100%;
	}
	.delete-answer-btn {
		display: flex;
		justify-content: center;
		align-items: center;
		width: 2rem;
		height: 2rem;
		padding: 0;
		border: none;
		border-radius: 0.25rem;
		background-color: var(--muted);
		color: var(--muted-foreground);
		cursor: pointer;
		transition: all 0.1s ease;
	}
	.delete-answer-btn:hover {
		background-color: var(--red-1);
		color: var(--err-foreground);
	}
	.delete-answer-btn svg {
		width: 1.5rem;
		height: 1.5rem;
		stroke-width: 2;
	}
	:global(.add-new-answer-btn) {
		margin: 1rem 0 0 0 !important;
		width: 100%;
	}
</style>
