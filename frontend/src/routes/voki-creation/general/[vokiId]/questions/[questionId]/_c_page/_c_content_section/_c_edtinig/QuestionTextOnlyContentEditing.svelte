<script lang="ts">
	import type { AnswerDataTextOnly, GeneralVokiCreationQuestionContent } from '../../../types';
	import AnswerEditingTextArea from './_c_shared/AnswerEditingTextArea.svelte';
	import QuestionContentEditingAnswersList from './_c_shared/QuestionContentEditingAnswersList.svelte';

	interface Props {
		content: Extract<GeneralVokiCreationQuestionContent, { $type: 'TextOnly' }>;
		maxAnswersForQuestionCount: number;
		resultsIdToName: Record<string, string>;
		maxResultsForAnswerCount: number;
		openRelatedResultsSelectingDialog: (
			selectedResultIds: string[],
			setSelected: (selected: string[]) => void
		) => void;
	}
	let {
		content = $bindable(),
		maxAnswersForQuestionCount,
		resultsIdToName,
		maxResultsForAnswerCount,
		openRelatedResultsSelectingDialog
	}: Props = $props();
	function addNewAnswer() {
		content.answers.push({
			text: '',
			relatedResultIds: [],
			order: content.answers.length
		});
	}
</script>

{#snippet answerContentSnippet(answer: AnswerDataTextOnly)}
	<div class="answer-content">
		<AnswerEditingTextArea bind:text={answer.text} />
	</div>
{/snippet}
<div class="question-content">
	<QuestionContentEditingAnswersList
		bind:answers={content.answers}
		{resultsIdToName}
		{answerContentSnippet}
		{maxAnswersForQuestionCount}
		{maxResultsForAnswerCount}
		{openRelatedResultsSelectingDialog}
		{addNewAnswer}
	/>
</div>

<style>
	.answer-content {
		display: flex;
		flex-direction: column;
		place-items: center center;
		width: 100%;
		height: 100%;
	}
</style>
