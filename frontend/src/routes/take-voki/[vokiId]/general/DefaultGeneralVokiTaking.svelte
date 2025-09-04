<script lang="ts">
	import { DefaultGeneralVokiTakingState } from './c_default_taking/default-general-voki-taking-state.svelte';
	import DefaultTakingButtonsContainer from './c_default_taking/DefaultTakingButtonsContainer.svelte';
	import DefaultTakingCurrentQuestionView from './c_default_taking/DefaultTakingCurrentQuestionView.svelte';
	import GeneralVokiReceivedResultView from './c_takings_shared/GeneralVokiReceivedResultView.svelte';
	import type { GeneralVokiTakingData, GeneralVokiTakingQuestionData } from './types';

	let { takingData }: { takingData: GeneralVokiTakingData } = $props<{
		takingData: GeneralVokiTakingData;
	}>();
	let vokiTakingState = new DefaultGeneralVokiTakingState(takingData);
	function isCurrentQuestionWithMultipleChoice() {
		const isSingle =
			vokiTakingState.currentQuestion!.minAnswersCount === 1 &&
			vokiTakingState.currentQuestion!.maxAnswersCount === 1;
		return !isSingle;
	}
</script>

{#if vokiTakingState.receivedResult === null}
	{#if vokiTakingState.currentQuestion}
		<DefaultTakingCurrentQuestionView
			question={vokiTakingState.currentQuestion}
			chosenAnswers={vokiTakingState.chosenAnswers[vokiTakingState.currentQuestion.id]}
			totalQuestionsCount={vokiTakingState.totalQuestionsCount()}
			isMultipleChoice={isCurrentQuestionWithMultipleChoice()}
		/>
	{:else}
		<h1>Question error</h1>
	{/if}
	<DefaultTakingButtonsContainer {vokiTakingState} />
{:else}
	<GeneralVokiReceivedResultView
		result={vokiTakingState.receivedResult}
		allowToSeeResultsList={true}
		vokiId={vokiTakingState.vokiId}
	/>
{/if}
