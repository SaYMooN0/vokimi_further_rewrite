<script lang="ts">
	import { ColorUtils } from '$lib/ts/utils/color-utils';
	import { onMount, onDestroy } from 'svelte';
	import type { GeneralVokiAnswerColorAndText, GeneralVokiAnswerTypeData } from '../../types';
	import { answersKeyboardNav, type AnswerRef } from './answers-keyboard-nav.svelte';
	import GeneralTakingAnswerChosenIndicator from './c_shared/GeneralTakingAnswerChosenIndicator.svelte';

	let {
		answers,
		isMultipleChoice,
		isAnswerChosen,
		chooseAnswer
	}: {
		answers: { typeData: GeneralVokiAnswerColorAndText; id: string }[];
		isMultipleChoice: boolean;
		isAnswerChosen: (answerId: string) => boolean;
		chooseAnswer: (answerId: string) => void;
	} = $props<{
		answers: { typeData: GeneralVokiAnswerTypeData; id: string }[];
		isMultipleChoice: boolean;
		isAnswerChosen: (answerId: string) => boolean;
		chooseAnswer: (answerId: string) => void;
	}>();

	let navAction = $state<ReturnType<typeof answersKeyboardNav>>()!;
	let answersContainer: HTMLDivElement = $state<HTMLDivElement>()!;
	export function focusFirstAnswerCard() {
		navAction?.focusFirstAnswer();
	}
	onMount(() => {
		navAction = answersKeyboardNav(answersContainer, {
			answers: answers as AnswerRef[],
			chooseAnswer,
			focusOnMount: true,
			useSpacebarToChoose: true
		});
	});
	onDestroy(() => navAction?.destroy?.());
</script>

<div
	class="answers-container"
	bind:this={answersContainer}
	tabindex="-1"
	role={isMultipleChoice ? 'group' : 'radiogroup'}
	aria-label="Answer choices"
>
	{#each answers as answer}
		<div
			class="answer"
			class:chosen={isAnswerChosen(answer.id)}
			onclick={() => chooseAnswer(answer.id)}
			tabindex="0"
			role={isMultipleChoice ? 'checkbox' : 'radio'}
			aria-checked={isAnswerChosen(answer.id)}
			style={`
        --chip:${ColorUtils.normalizeHex6(answer.typeData.color) ?? answer.typeData.color};
        --chip-text:${ColorUtils.contrastTextColor(answer.typeData.color)};
      `}
		>
			<GeneralTakingAnswerChosenIndicator {isMultipleChoice} isChosen={isAnswerChosen(answer.id)} />
			<div class="color-chip"><span class="chip-text">{answer.typeData.color}</span></div>
			<label class="text">{answer.typeData.text}</label>
			<div class="color-chip-thin"></div>
		</div>
	{/each}
</div>

<style>
	.answers-container {
		display: grid;
		gap: 0.75rem;
	}

	.answer {
		display: grid;
		align-items: center;
		gap: 0.75rem;
		height: fit-content;
		padding: 0.5rem 1rem;
		border-radius: 0.5rem;
		grid-template-columns: auto 1fr;
		grid-template-columns: auto auto 1fr auto;
	}

	.color-chip {
		display: grid;
		width: 10rem;
		height: 3rem;
		padding: 0 0.5rem;
		border-radius: 0.5rem;
		background: var(--chip);
		color: var(--chip-text);
		place-items: center;
		white-space: nowrap;
	}

	.chip-text {
		font-size: 0.75rem;
		font-weight: 600;
		opacity: 0.95;
	}

	.color-chip-thin {
		width: 1rem;
		height: 100%;
		border-radius: 0.5rem;
		background: var(--chip);
	}
</style>
