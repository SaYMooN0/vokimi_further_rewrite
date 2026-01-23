<script lang="ts" generics="T extends BaseGeneralVokiAnswerData">
	import QuestionContentEditingAnswerResults from './_c_answers_list/QuestionContentEditingAnswerResults.svelte';
	import type { Snippet } from 'svelte';
	import type { BaseGeneralVokiAnswerData } from '../../../../types';
	import QuestionContentEditingNoAnswers from './_c_answers_list/QuestionContentEditingNoAnswers.svelte';
	import SingleAnswerWrapper from '../../_c_shared/SingleAnswerWrapper.svelte';

	interface Props {
		answers: T[];
		answerContentSnippet: Snippet<[T]>;
		maxAnswersForQuestionCount: number;
		addNewAnswer: () => void;
		resultsIdToName: Record<string, string>;
		maxResultsForAnswerCount: number;
		openRelatedResultsSelectingDialog: (
			selectedResultIds: string[],
			setSelected: (selected: string[]) => void
		) => void;
	}
	let {
		answers = $bindable(),
		answerContentSnippet,
		maxAnswersForQuestionCount,
		addNewAnswer,
		resultsIdToName,
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
		{#each answers as answer}
			<SingleAnswerWrapper>
				{#snippet results()}
					<QuestionContentEditingAnswerResults
						answerResults={answer.relatedResultIds}
						setAnswerResults={(results) => (answer.relatedResultIds = results)}
						removeResultFromAnswer={(resId) => removeResultFromAnswer(answer, resId)}
						{resultsIdToName}
						{maxResultsForAnswerCount}
						{openRelatedResultsSelectingDialog}
					/>
				{/snippet}
				{#snippet answerContent()}
					{@render answerContentSnippet(answer)}
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
