<script lang="ts">
	import GeneralVokiTakingAnswersDisplay from '../_c_takings_shared/GeneralVokiTakingAnswersDisplay.svelte';
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
	text={question.text}
	imageKeys={question.imageKeys}
	imagesAspectRatio={question.imagesAspectRatio}
	minAnswersCount={question.minAnswersCount}
	maxAnswersCount={question.maxAnswersCount}
	{totalQuestionsCount}
	questionNumber={question.orderInVokiTaking + 1}
/>
<GeneralVokiTakingAnswersDisplay
	answerType={question.answerType}
	answers={question.answers}
	bind:chosenAnswers
	{isMultipleChoice}
/>
