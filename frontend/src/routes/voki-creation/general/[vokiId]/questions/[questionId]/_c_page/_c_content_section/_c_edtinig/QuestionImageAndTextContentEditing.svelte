<script lang="ts">
	import type { AnswerDataImageAndText, GeneralVokiCreationQuestionContent } from '../../../types';
	import type { QuestionPageResultsState } from '../../../general-voki-creation-specific-question-page-state.svelte';
	import QuestionContentEditingAnswersList from './_c_shared/QuestionContentEditingAnswersList.svelte';
	import ImageAndTextAnswerEditing from './_c_answers_content/ImageAndTextAnswerEditing.svelte';

	interface Props {
		content: Extract<GeneralVokiCreationQuestionContent, { $type: 'ImageAndText' }>;
		maxAnswersForQuestionCount: number;
		resultsIdToNameState: QuestionPageResultsState;
		maxResultsForAnswerCount: number;
		openRelatedResultsSelectingDialog: (
			selectedResultIds: string[],
			setSelected: (selected: string[]) => void
		) => void;
	}
	let {
		content = $bindable(),
		maxAnswersForQuestionCount,
		resultsIdToNameState,
		maxResultsForAnswerCount,
		openRelatedResultsSelectingDialog
	}: Props = $props();
	function addNewAnswer() {
		content.answers.push({
			text: '',
			image: '',
			relatedResultIds: [],
			order: content.answers.length + 1
		});
	}
</script>

{#snippet answerMainContent(
	answer: AnswerDataImageAndText,
	updateOnChange: (newAnswer: AnswerDataImageAndText) => void
)}
	<ImageAndTextAnswerEditing {answer} {updateOnChange} />
{/snippet}
<div class="question-content">
	<QuestionContentEditingAnswersList
		bind:answers={content.answers}
		{resultsIdToNameState}
		{answerMainContent}
		{maxAnswersForQuestionCount}
		{maxResultsForAnswerCount}
		{openRelatedResultsSelectingDialog}
		{addNewAnswer}
	/>
</div>

<style>
	.question-content {
		display: flex;
		flex-direction: column;
		width: 100%;
	}
</style>
