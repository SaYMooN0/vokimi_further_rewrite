<script lang="ts">
	import type { Err } from '$lib/ts/err';
	import { DefaultGeneralVokiTakingState } from './c_default_taking/default-general-voki-taking-state.svelte';
	import DefaultGeneralVokiTakingErrsList from './c_default_taking/DefaultGeneralVokiTakingErrsList.svelte';
	import DefaultTakingButtonsContainer from './c_default_taking/DefaultTakingButtonsContainer.svelte';
	import DefaultTakingCurrentQuestionView from './c_default_taking/DefaultTakingCurrentQuestionView.svelte';
	import GeneralVokiReceivedResultView from './c_takings_shared/GeneralVokiReceivedResultView.svelte';
	import type { GeneralVokiTakenResult, GeneralVokiTakingData } from './types';

	interface Props {
		takingData: GeneralVokiTakingData;
		clearVokiSeenUpdateTimer: () => void;
	}
	let { takingData, clearVokiSeenUpdateTimer }: Props = $props();

	let vokiTakingState = new DefaultGeneralVokiTakingState(takingData, clearVokiSeenUpdateTimer);

	type ErrWithOrder = Err & { questionOrder?: number };
	let vokiTakingErrs = $state<ErrWithOrder[]>([]);

	let vokiTakenData: GeneralVokiTakenResult | null = $state(null);

	function isCurrentQuestionWithMultipleChoice() {
		const isSingle =
			vokiTakingState.currentQuestion!.minAnswersCount === 1 &&
			vokiTakingState.currentQuestion!.maxAnswersCount === 1;
		return !isSingle;
	}
	function jumpToSpecificQuestionFromErrsList(questionOrder: number): Err[] {
		vokiTakingErrs = [];
		return vokiTakingState.jumpToSpecificQuestion(questionOrder);
	}
</script>

{#if vokiTakenData === null}
	{#if vokiTakingState.currentQuestion}
		<DefaultTakingCurrentQuestionView
			question={vokiTakingState.currentQuestion}
			bind:chosenAnswers={vokiTakingState.chosenAnswers[vokiTakingState.currentQuestion.id]}
			totalQuestionsCount={vokiTakingState.totalQuestionsCount}
			isMultipleChoice={isCurrentQuestionWithMultipleChoice()}
		/>
	{:else}
		<h1>Question error</h1>
	{/if}
	<DefaultGeneralVokiTakingErrsList
		errs={vokiTakingErrs}
		jumpToSpecificQuestion={jumpToSpecificQuestionFromErrsList}
	/>
	<DefaultTakingButtonsContainer
		{vokiTakingState}
		bind:vokiTakingErrs
		onResultReceived={(result) => (vokiTakenData = result)}
	/>
{:else}
	<GeneralVokiReceivedResultView
		result={vokiTakenData.receivedResult}
		allowToSeeResultsList={true}
		vokiId={vokiTakingState.vokiId}
	/>
{/if}
