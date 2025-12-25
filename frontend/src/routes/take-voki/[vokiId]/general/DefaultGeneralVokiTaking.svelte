<script lang="ts">
	import type { Err } from '$lib/ts/err';
	import { onMount, onDestroy } from 'svelte';
	import { DefaultGeneralVokiTakingState } from './_c_default_taking/default-general-voki-taking-state.svelte';
	import { createQuestionsKeyHandler } from './_c_default_taking/default-voki-taking-questions-nav';
	import DefaultGeneralVokiTakingErrsList from './_c_default_taking/DefaultGeneralVokiTakingErrsList.svelte';
	import DefaultTakingButtonsContainer from './_c_default_taking/DefaultTakingButtonsContainer.svelte';
	import DefaultTakingCurrentQuestionView from './_c_default_taking/DefaultTakingCurrentQuestionView.svelte';
	import type { GeneralVokiTakingData } from './types';

	interface Props {
		takingData: GeneralVokiTakingData;
		clearVokiSeenUpdateTimer: () => void;
		onResultReceived: (resultId: string) => void;
	}
	let { takingData, clearVokiSeenUpdateTimer, onResultReceived }: Props = $props();

	let vokiTakingState = new DefaultGeneralVokiTakingState(takingData, clearVokiSeenUpdateTimer);

	type ErrWithOrder = Err & { questionOrder?: number };
	let vokiTakingErrs = $state<ErrWithOrder[]>([]);

	function jumpToSpecificQuestionFromErrsList(questionOrder: number): Err[] {
		vokiTakingErrs = [];
		return vokiTakingState.jumpToSpecificQuestion(questionOrder);
	}

	let answersContainer: { focusFirstAnswerCard: () => void } = $state<{
		focusFirstAnswerCard: () => void;
	}>()!;

	onMount(() => {
		const handler = createQuestionsKeyHandler({
			goToNextQuestion: () => vokiTakingState.goToNextQuestion(),
			goToPreviousQuestion: () => vokiTakingState.goToPreviousQuestion(),
			focusFirstAnswerCardInContainer: () => answersContainer.focusFirstAnswerCard()
		});

		window.addEventListener('keydown', handler);
		onDestroy(() => window.removeEventListener('keydown', handler));
	});
</script>

<div class="taking-container">
	{#if vokiTakingState.currentQuestion}
		<DefaultTakingCurrentQuestionView
			bind:this={answersContainer}
			question={vokiTakingState.currentQuestion}
			bind:chosenAnswers={vokiTakingState.chosenAnswers[vokiTakingState.currentQuestion.id]}
			totalQuestionsCount={vokiTakingState.totalQuestionsCount}
		/>
	{:else}
		<h1>Question error</h1>
		<button onclick={() => vokiTakingState.jumpToSpecificQuestion(1)}>Go to first question</button>
	{/if}

	<DefaultGeneralVokiTakingErrsList
		errs={vokiTakingErrs}
		jumpToSpecificQuestion={jumpToSpecificQuestionFromErrsList}
	/>
	<DefaultTakingButtonsContainer {vokiTakingState} bind:vokiTakingErrs {onResultReceived} />
</div>
