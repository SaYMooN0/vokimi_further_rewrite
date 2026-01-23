<script lang="ts">
	import type { AnswerDataImageAndText, GeneralVokiCreationQuestionContent } from '../../../types';
	import ImageAndTextAnswerContentEditing from './_c_image_and_text/ImageAndTextAnswerContentEditing.svelte';
	import AnswerEditingTextArea from './_c_shared/AnswerEditingTextArea.svelte';
	import QuestionContentEditingAnswersList from './_c_shared/QuestionContentEditingAnswersList.svelte';

	interface Props {
		content: Extract<GeneralVokiCreationQuestionContent, { $type: 'ImageAndText' }>;
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
			image: '',
			relatedResultIds: [],
			order: content.answers.length
		});
	}
</script>

{#snippet answerContentSnippet(answer: AnswerDataImageAndText)}
	<ImageAndTextAnswerContentEditing {answer} />
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
