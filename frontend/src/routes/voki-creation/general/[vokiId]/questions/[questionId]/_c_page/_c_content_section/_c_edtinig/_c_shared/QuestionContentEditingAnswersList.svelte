<script lang="ts" generics="T extends BaseGeneralVokiAnswerData">
	import QuestionContentEditingAnswerResults from './_c_answers_list/QuestionContentEditingAnswerResults.svelte';
	import type { Snippet } from 'svelte';
	import type { BaseGeneralVokiAnswerData } from '../../../../types';
	import type { QuestionPageResultsState } from '../../../../general-voki-creation-specific-question-page-state.svelte';
	import QuestionContentEditingNoAnswers from './_c_answers_list/QuestionContentEditingNoAnswers.svelte';
	import SingleAnswerWrapper from '../../_c_shared/SingleAnswerWrapper.svelte';

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
</script>

{#if answers.length === 0}
	<QuestionContentEditingNoAnswers {addNewAnswer} />
{:else}
	<div class="answer-list">
		{#each answers as answer, answerKey}
			<SingleAnswerWrapper
				resultsIdToName={resultsIdToNameState}
				answerRelatedResultsCount={answer.relatedResultIds.length}
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
					{@render answerMainContent(answer, (newAnswer) => {
						answers[answerKey] = newAnswer;
					})}
				{/snippet}
			</SingleAnswerWrapper>
		{/each}
		{#if answers.length < maxAnswersForQuestionCount}
			<button onclick={addNewAnswer}>Add new answer</button>
		{:else}
			Answers count limit for question reached ({answers.length}/{maxAnswersForQuestionCount})
		{/if}
	</div>
{/if}

<style>
</style>
