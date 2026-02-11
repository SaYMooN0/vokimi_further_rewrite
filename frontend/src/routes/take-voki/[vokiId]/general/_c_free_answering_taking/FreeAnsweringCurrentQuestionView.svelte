<script lang="ts">
	import GeneralVokiTakingQuestionDisplay from '../_c_takings_shared/GeneralVokiTakingQuestionDisplay.svelte';
	import type { GeneralVokiTakingQuestionData } from '../types';

	interface Props {
		question: GeneralVokiTakingQuestionData;
		chosenAnswers: Record<string, boolean>;
		totalQuestionsCount: number;
	}
	let { question, chosenAnswers = $bindable(), totalQuestionsCount }: Props = $props();

	let isMultipleChoice = $derived(question.minAnswersCount != 1 || question.maxAnswersCount != 1);

	let answersContainer: { focusFirstAnswerCard: () => void } = $state<{
		focusFirstAnswerCard: () => void;
	}>()!;
	export function focusFirstAnswerCard() {
		answersContainer.focusFirstAnswerCard();
	}
</script>

<GeneralVokiTakingQuestionDisplay
	{question}
	{totalQuestionsCount}
	questionChosenAnswers={chosenAnswers}
	isQuestionMultipleChoice={isMultipleChoice}
	content={question.content}
/>
