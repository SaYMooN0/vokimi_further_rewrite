<script lang="ts">
	import GeneralVokiTakingAnswersDisplay from '../c_takings_shared/GeneralVokiTakingAnswersDisplay.svelte';
	import GeneralVokiTakingQuestionDisplay from '../c_takings_shared/GeneralVokiTakingQuestionDisplay.svelte';
	import type { GeneralVokiTakingQuestionData } from '../types';

	interface Props {
		question: GeneralVokiTakingQuestionData;
		chosenAnswers: Record<string, boolean>;
		totalQuestionsCount: number;
		isMultipleChoice: boolean;
	}
	let {
		question,
		chosenAnswers = $bindable(),
		totalQuestionsCount,
		isMultipleChoice
	}: Props = $props();
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
