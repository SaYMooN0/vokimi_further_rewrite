<script lang="ts">
	import type { GeneralVokiAnswerType } from '$lib/ts/voki';
	import type { SvelteComponent } from 'svelte';
	import type { GeneralVokiAnswerTypeData } from '../types';
	import GeneralVokiTakingAudioAndTextAnswer from './c_answers_display/GeneralVokiTakingAudioAndTextAnswer.svelte';
	import GeneralVokiTakingAudioOnlyAnswer from './c_answers_display/GeneralVokiTakingAudioOnlyAnswer.svelte';
	import GeneralVokiTakingColorAndTextAnswer from './c_answers_display/GeneralVokiTakingColorAndTextAnswer.svelte';
	import GeneralVokiTakingColorOnlyAnswer from './c_answers_display/GeneralVokiTakingColorOnlyAnswer.svelte';
	import GeneralVokiTakingImageAndTextAnswer from './c_answers_display/GeneralVokiTakingImageAndTextAnswer.svelte';
	import GeneralVokiTakingImageOnlyAnswer from './c_answers_display/GeneralVokiTakingImageOnlyAnswer.svelte';
	import GeneralVokiTakingTextOnlyAnswer from './c_answers_display/GeneralVokiTakingTextOnlyAnswer.svelte';

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
			Object.keys(chosenAnswers).forEach((key, index) => {
				chosenAnswers[key] = false;
			});
			chosenAnswers[answerId] = true;
		}
	}
	function isAnswerChosen(answerId: string) {
		return chosenAnswers[answerId];
	}
	let { answerType, answers, chosenAnswers = $bindable(), isMultipleChoice }: Props = $props();

	export function focusFirstAnswerCard() {
		answersContainer.focusFirstAnswerCard();
	}
	let answersContainer: { focusFirstAnswerCard: () => void } = $state<{
		focusFirstAnswerCard: () => void;
	}>()!;
</script>

{#if answerType === 'TextOnly'}
	<GeneralVokiTakingTextOnlyAnswer {answers} {isMultipleChoice} {isAnswerChosen} {chooseAnswer} />
{:else if answerType === 'ImageOnly'}
	<GeneralVokiTakingImageOnlyAnswer {answers} {isMultipleChoice} {isAnswerChosen} {chooseAnswer} />
{:else if answerType === 'ImageAndText'}
	<GeneralVokiTakingImageAndTextAnswer
		{answers}
		{isMultipleChoice}
		{isAnswerChosen}
		{chooseAnswer}
	/>
{:else if answerType === 'ColorOnly'}
	<GeneralVokiTakingColorOnlyAnswer
		bind:this={answersContainer}
		{answers}
		{isMultipleChoice}
		{isAnswerChosen}
		{chooseAnswer}
	/>
{:else if answerType === 'ColorAndText'}
	<GeneralVokiTakingColorAndTextAnswer
		{answers}
		{isMultipleChoice}
		{isAnswerChosen}
		{chooseAnswer}
	/>
{:else if answerType === 'AudioOnly'}
	<GeneralVokiTakingAudioOnlyAnswer {answers} {isMultipleChoice} {isAnswerChosen} {chooseAnswer} />
{:else if answerType === 'AudioAndText'}
	<GeneralVokiTakingAudioAndTextAnswer
		{answers}
		{isMultipleChoice}
		{isAnswerChosen}
		{chooseAnswer}
	/>
{:else}
	<div class="anwer-type-error">
		<h1>This anser type is not supported</h1>
	</div>
{/if}

<style>
	:global(.answers-container) {
		scroll-behavior: smooth;
	}

	:global(.answers-container > .answer) {
		box-shadow: var(--shadow), var(--shadow-xs);
		transition: transform 0.18s ease;
		scroll-margin: 12vh 0;
	}

	:global(.answers-container > .answer:hover),
	:global(.answers-container > .answer:focus),
	:global(.answers-container > .answer.chosen) {
		transition: transform 0.25s ease;
		transform: scale(1.009);
	}
</style>
