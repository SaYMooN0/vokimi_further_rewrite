<script lang="ts">
	import { StorageBucketMain } from '$lib/ts/backend-communication/storage-buckets';
	import type { GeneralVokiAnswerTypeData, GeneralVokiAnswerImageOnly } from '../../types';
	import { type AnswerRef, answersKeyboardNav } from './answers-keyboard-nav.svelte';
	import GeneralTakingAnswerChosenIndicator from './c_shared/GeneralTakingAnswerChosenIndicator.svelte';

	let {
		answers,
		isMultipleChoice,
		isAnswerChosen,
		chooseAnswer
	}: {
		answers: { typeData: GeneralVokiAnswerImageOnly; id: string }[];
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

			<GeneralTakingAnswerChosenIndicator {isMultipleChoice} isChosen={isAnswerChosen(answer.id)} />
		</div>
	{/each}
</div>

<style>
	.answers-container {
		display: grid;
		justify-content: center;
		gap: 1.5rem;
		grid-template-columns: repeat(auto-fill, minmax(25rem, 1fr));
	}

	.answer {
		display: grid;
		gap: 0.5rem;
		padding: 1rem 0.5rem;
		border-radius: 1rem;
		background-color: var(--back);
		grid-template-rows: 1fr auto;
		justify-items: center;
	}

	.img-container {
		display: flex;
		justify-content: center;
		align-items: center;
		width: 100%;
		width: fit-content;
		height: 100%;
	}

	img {
		min-width: 16rem;
		max-width: 25rem;
		max-height: 18rem;
		border-radius: 0.75rem;
		object-fit: contain;
		-webkit-user-drag: none;
		box-shadow: var(--shadow-xs);
	}
</style>
