<script lang="ts">
	import { StorageBucketMain } from '$lib/ts/backend-communication/storage-buckets';
	import type { GeneralVokiAnswerImageAndText, GeneralVokiAnswerTypeData } from '../../types';
	import { type AnswerRef, answersKeyboardNav } from './answers-keyboard-nav.svelte';
	import GeneralTakingAnswerChosenIndicator from './c_shared/GeneralTakingAnswerChosenIndicator.svelte';
	import GeneralTakingAnswerText from './c_shared/GeneralTakingAnswerText.svelte';

	let {
		answers,
		isMultipleChoice,
		isAnswerChosen,
		chooseAnswer
	}: {
		answers: { typeData: GeneralVokiAnswerImageAndText; id: string }[];
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
			<div class="img-container">
				<img
					class="unselectable"
					src={StorageBucketMain.fileSrc(answer.typeData.image)}
					alt="Answer"
					draggable="false"
				/>
			</div>

			<GeneralTakingAnswerText text={answer.typeData.text} />

			<GeneralTakingAnswerChosenIndicator {isMultipleChoice} isChosen={isAnswerChosen(answer.id)} />
		</div>
	{/each}
</div>

<style>
	.answers-container {
		display: grid;
		justify-content: center;
		gap: 1.5rem;
		grid-template-columns: repeat(auto-fill, minmax(34ch, 1fr));
	}

	.answer {
		display: grid;
		gap: 0.5rem;
		padding: 1rem 0.5rem;
		border-radius: 1rem;
		background-color: var(--back);
		grid-template-rows: auto 1fr auto;
		justify-items: center;
	}

	.img-container {
		display: flex;
		justify-content: center;
		align-items: center;
		width: fit-content;
	}

	img {
		display: block;
		width: 100%;
		height: 100%;
		max-height: 20rem;
		border-radius: 0.75rem;
		object-fit: contain;
		-webkit-user-drag: none;
        box-shadow: var(--shadow-xs);
	}
</style>
