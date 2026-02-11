<script lang="ts">
	import GeneralVokiTakingAnswersDisplay from '../_c_takings_shared/_c_question_display/GeneralVokiTakingQuestionContentDisplay.svelte';
	import GeneralVokiTakingQuestionDisplay from '../_c_takings_shared/GeneralVokiTakingQuestionDisplay.svelte';
	import type { GeneralVokiTakingQuestionData } from '../types';

	interface Props {
		question: GeneralVokiTakingQuestionData;
		chosenAnswers: Record<string, boolean>;
		totalQuestionsCount: number;
	}
	let { question, chosenAnswers = $bindable(), totalQuestionsCount }: Props = $props();
	let isMultipleChoice = $derived(question.minAnswersCount != 1 || question.maxAnswersCount != 1);
</script>

<GeneralVokiTakingQuestionDisplay
	{question}
	{totalQuestionsCount}
	questionChosenAnswers={chosenAnswers}
	isQuestionMultipleChoice={isMultipleChoice}
	content={question.content}
/>
