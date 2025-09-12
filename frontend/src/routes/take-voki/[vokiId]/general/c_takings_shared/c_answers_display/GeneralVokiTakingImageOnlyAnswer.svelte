<script lang="ts">
	import { StorageBucketMain } from '$lib/ts/backend-communication/storage-buckets';
	import type { GeneralVokiAnswerTypeData, GeneralVokiAnswerImageOnly } from '../../types';
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
</script>

<div class="answers-container">
	{#each answers as answer}
		<div
			class="answer"
			class:chosen={isAnswerChosen(answer.id)}
			onclick={() => chooseAnswer(answer.id)}
		>
			<img
				class="unselectable"
				src={StorageBucketMain.fileSrc(answer.typeData.image)}
				alt="Answer"
				decoding="async"
				draggable="false"
			/>
			<GeneralTakingAnswerChosenIndicator {isMultipleChoice} isChosen={isAnswerChosen(answer.id)} />
		</div>
	{/each}
</div>

<style>
	.answers-container {
		display: flex;
		flex-wrap: wrap;
		gap: 1.5rem;
		justify-content: center;
	}

	.answer {
		width: fit-content;
		flex: 1 1 20rem;
		border-radius: 1rem;
		display: flex;
		grid-template-rows: 1fr;
		background-color: var(--back);
		padding: 0.5rem 0.375rem 1rem;
		gap: 0.5rem;
		flex-direction: column;
		align-items: center;
		justify-content: space-between;
	}

	img {
		max-height: 18rem;
		min-width: 16rem;
		max-width: 28rem;
		object-fit: contain;
		display: block;
		-webkit-user-drag: none;
		border-radius: 0.75rem;
	}
</style>
