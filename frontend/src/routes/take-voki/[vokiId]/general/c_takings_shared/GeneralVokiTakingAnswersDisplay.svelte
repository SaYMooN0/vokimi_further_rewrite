<script lang="ts">
	import type { GeneralVokiAnswerType } from '$lib/ts/voki';
	import type { GeneralVokiAnswerTypeData } from '../types';
	import GeneralVokiTakingAnswersDisplay from './c_answers_display/GeneralVokiTakingTextOnlyAnswers.svelte';

	interface Props {
		answerType: GeneralVokiAnswerType;
		answers: { typeData: GeneralVokiAnswerTypeData; id: string }[];
		chosenAnswers: Record<string, boolean>;
		isMultipleChoice: boolean;
	}
	function chooseAnswer(answerId: string) {
		if (isMultipleChoice) {
			chosenAnswers[answerId] = !chosenAnswers[answerId];
		} else {
			const currChosenAnswerId = Object.keys(chosenAnswers).find((k) => chosenAnswers[k]);
			if (currChosenAnswerId) {
				chosenAnswers[currChosenAnswerId] = false;
			}
			chosenAnswers[answerId] = true;
		}
	}
	function isAnswerChosen(answerId: string) {
		return chosenAnswers[answerId];
	}
	let { answerType, answers, chosenAnswers = $bindable(), isMultipleChoice }: Props = $props();
</script>

{#if answerType === 'TextOnly'}
	<GeneralVokiTakingAnswersDisplay {answers} {isMultipleChoice} {isAnswerChosen} {chooseAnswer} />
{:else}
	<h1>Unknown answer type</h1>
{/if}
