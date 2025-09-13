<script lang="ts">
	import { ColorUtils } from '$lib/ts/utils/color-utils';
	import type { GeneralVokiAnswerTypeData, GeneralVokiAnswerColorOnly } from '../../types';
	import { type AnswerRef, answersKeyboardNav } from './answers-keyboard-nav.svelte';
	import GeneralTakingAnswerChosenIndicator from './c_shared/GeneralTakingAnswerChosenIndicator.svelte';

	let {
		answers,
		isMultipleChoice,
		isAnswerChosen,
		chooseAnswer
	}: {
		answers: { typeData: GeneralVokiAnswerColorOnly; id: string }[];
		isMultipleChoice: boolean;
		isAnswerChosen: (answerId: string) => boolean;
		chooseAnswer: (answerId: string) => void;
	} = $props<{
		answers: { typeData: GeneralVokiAnswerTypeData; id: string }[];
		isMultipleChoice: boolean;
		isAnswerChosen: (answerId: string) => boolean;
		chooseAnswer: (answerId: string) => void;
	}>();

	let container: HTMLDivElement = $state<HTMLDivElement>()!;
</script>

<div
	class="answers-container"
	bind:this={container}
	use:answersKeyboardNav={{
		answers: answers as AnswerRef[],
		chooseAnswer,
		focusOnMount: true,
		useSpacebarToChoose: true
	}}
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
		>
			<div
				class="color-div"
				style="background-color:{ColorUtils.normalizeHex6(answer.typeData.color) ??
					answer.typeData.color};"
			>
				<div class="indicator-container">
					<GeneralTakingAnswerChosenIndicator
						{isMultipleChoice}
						isChosen={isAnswerChosen(answer.id)}
					/>
				</div>
			</div>
		</div>
	{/each}
</div>

<style>
	.answers-container {
		display: flex;
		flex-wrap: wrap;
		justify-content: center;
		gap: 2rem;
	}
	.answer {
		flex: 0 1 18rem;
		display: grid;
		height: 7rem;
		padding: 1rem;
		gap: 0.5rem;
		border-radius: 1rem;
	}
	.color-div {
		--color-div-border-radius: 0.675rem;
		height: 100%;
		width: 100%;
		border-radius: var(--color-div-border-radius);
		position: relative;
		border: 1px solid var(--muted);
	}
	.indicator-container {
		position: absolute;
		bottom: -1rem;
		background-color: var(--back);
		padding: 0.5rem;
		border-radius: var(--color-div-border-radius);
		border-top: 1px solid var(--muted);
		left: 50%;
		transform: translateX(-50%);
	}
</style>
