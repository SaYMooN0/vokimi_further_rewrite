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
		display: grid;
		gap: 0.5rem;
		height: 7rem;
		padding: 1rem;
		border-radius: 1rem;
		flex: 0 1 18rem;
	}

	.color-div {
		--color-div-border-radius: 0.675rem;

		position: relative;
		width: 100%;
		height: 100%;
		border: 1px solid var(--muted);
		border-radius: var(--color-div-border-radius);
	}

	.indicator-container {
		position: absolute;
		bottom: -1rem;
		left: 50%;
		padding: 0.5rem;
		border-radius: var(--color-div-border-radius);
		background-color: var(--back);
		transform: translateX(-50%);
		border-top: 1px solid var(--muted);
	}
</style>
