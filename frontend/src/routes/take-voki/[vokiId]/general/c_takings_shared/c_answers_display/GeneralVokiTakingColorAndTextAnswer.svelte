<script lang="ts">
	import { ColorUtils } from '$lib/ts/utils/color-utils';
	import type { GeneralVokiAnswerColorAndText, GeneralVokiAnswerTypeData } from '../../types';
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
</script>

<div class="answers-container">
	{#each answers as answer}
		<div
			class="answer"
			class:chosen={isAnswerChosen(answer.id)}
			onclick={() => chooseAnswer(answer.id)}
			role="button"
			tabindex="0"
			aria-pressed={isAnswerChosen(answer.id)}
			style={`
					--chip:${ColorUtils.normalizeHex6(answer.typeData.color) ?? answer.typeData.color};
					--chipText:${ColorUtils.contrastTextColor(answer.typeData.color)};
				`}
		>
			<GeneralTakingAnswerChosenIndicator {isMultipleChoice} isChosen={isAnswerChosen(answer.id)} />

			<div class="color-chip">
				<span class="chip-text">{answer.typeData.color}</span>
			</div>

			<label class="text">
				{answer.typeData.text}
			</label>
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
		grid-template-columns: auto 1fr;
		align-items: center;
		gap: 0.75rem;
		padding: 0.5rem 1rem;
		grid-template-columns: auto auto 1fr auto;
		align-items: center;
		gap: 0.75rem;
		height: fit-content;
		border-radius: 0.5rem;
	}
	.color-chip {
		width: 10rem;
		height: 3rem;
		border-radius: 0.5rem;
		background: var(--chip);
		color: var(--chipText);
		display: grid;
		place-items: center;
		padding: 0 0.5rem;
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
		background: var(--chip);
		border-radius: 0.5rem;
	}
	.text {
		font-size: 1.25rem;
		font-weight: 450;
		cursor: default;
		word-break: normal;
		overflow-wrap: anywhere;
	}
</style>
