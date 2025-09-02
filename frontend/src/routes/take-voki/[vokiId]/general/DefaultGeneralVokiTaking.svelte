<script lang="ts">
	import { DefaultGeneralVokiTakingState } from './c_default_taking/default-general-voki-taking-state.svelte';
	import DefaultTakingButtonsContainer from './c_default_taking/DefaultTakingButtonsContainer.svelte';
	import DefaultTakingCurrentQuestionView from './c_default_taking/DefaultTakingCurrentQuestionView.svelte';
	import GeneralVokiReceivedResultView from './c_takings_shared/GeneralVokiReceivedResultView.svelte';
	import type { GeneralVokiTakingData } from './types';

	let { takingData }: { takingData: GeneralVokiTakingData } = $props<{
		takingData: GeneralVokiTakingData;
	}>();
	let vokiTakingState = new DefaultGeneralVokiTakingState(takingData);
</script>

{#if vokiTakingState.receivedResult === null}
	{#if vokiTakingState.currentQuestion}
		<DefaultTakingCurrentQuestionView
			question={vokiTakingState.currentQuestion}
			chosenAnswers={vokiTakingState.chosenAnswers.get(vokiTakingState.currentQuestion.id)!}
			totalQuestionsCount={vokiTakingState.totalQuestionsCount()}
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
