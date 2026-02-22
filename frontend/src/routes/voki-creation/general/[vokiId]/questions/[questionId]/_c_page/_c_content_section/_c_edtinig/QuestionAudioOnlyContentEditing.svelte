<script lang="ts">
	import type { AnswerDataAudioOnly, GeneralVokiCreationQuestionContent } from '../../../types';
	import type { QuestionPageResultsState } from '../../../general-voki-creation-specific-question-page-state.svelte';
	import QuestionContentEditingAnswersList from './_c_shared/QuestionContentEditingAnswersList.svelte';
	import AudioOnlyAnswerEditing from './_c_answers_content/AudioOnlyAnswerEditing.svelte';

	interface Props {
		content: Extract<GeneralVokiCreationQuestionContent, { $type: 'AudioOnly' }>;
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
			audio: '',
			relatedResultIds: [],
			order: content.answers.length + 1
		});
	}
</script>

{#snippet answerMainContent(getAnswer: () => AnswerDataAudioOnly)}
	<AudioOnlyAnswerEditing
		answer={getAnswer()}
		onAudioChange={(newAudio) => (getAnswer().audio = newAudio)}
	/>
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
