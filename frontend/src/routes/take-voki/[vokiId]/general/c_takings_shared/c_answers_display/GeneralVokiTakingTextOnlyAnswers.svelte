<script lang="ts">
	import type { GeneralVokiAnswerTextOnly, GeneralVokiAnswerTypeData } from '../../types';
	import GeneralTakingAnswerChosenIndicator from './c_shared/GeneralTakingAnswerChosenIndicator.svelte';

	let {
		answers,
		isMultipleChoice,
		isAnswerChosen,
		chooseAnswer
	}: {
		answers: { typeData: GeneralVokiAnswerTextOnly; id: string }[];
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
		>
			<GeneralTakingAnswerChosenIndicator {isMultipleChoice} isChosen={isAnswerChosen(answer.id)} />
			<label>
				{answer.typeData.text}
			</label>
		</div>
	{/each}
</div>

<style>
	.answer {
		display: grid;
		align-items: center;
		gap: 0.5rem;
		padding: 0.5rem 1rem;
		margin: 1rem 0;
		border-radius: 0.5rem;
		box-shadow: var(--shadow), var(--shadow-xs);
		transition: transform 0.18s ease;
		grid-template-columns: auto 1fr;
	}

	.answer > label {
		font-size: 1.125rem;
		font-weight: 400;
		cursor: default;
		word-break: normal;
		overflow-wrap: anywhere;
		text-indent: 0.25em;
	}

	.answer:hover,
	.answer.chosen {
		transition: transform 0.25s ease;
		transform: scale(1.009);
	}
</style>
