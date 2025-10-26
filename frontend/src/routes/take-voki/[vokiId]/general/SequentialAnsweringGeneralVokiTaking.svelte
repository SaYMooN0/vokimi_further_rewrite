<script lang="ts">
	import { SequentialAnsweringGeneralVokiTakingState } from './c_sequential_answering_taking/sequential-answering-general-voki-taking-state.svelte';
	import SequentialAnsweringCurrentQuestionView from './c_sequential_answering_taking/SequentialAnsweringCurrentQuestionView.svelte';
	import SequentialAnsweringNavigationContainer from './c_sequential_answering_taking/SequentialAnsweringNavigationContainer.svelte';
	import type { GeneralVokiTakingData } from './types';

	interface Props {
		takingData: GeneralVokiTakingData;
		clearVokiSeenUpdateTimer: () => void;
		onResultReceived: (resultId: string) => void;
	}
	let { takingData, clearVokiSeenUpdateTimer, onResultReceived }: Props = $props();

	let vokiTakingState = new SequentialAnsweringGeneralVokiTakingState(
		takingData,
		clearVokiSeenUpdateTimer
	);

</script>

{#if vokiTakingState.currentQuestion}
	<SequentialAnsweringCurrentQuestionView
		question={vokiTakingState.currentQuestion}
		bind:chosenAnswers={vokiTakingState.currentQuestionChosenAnswers}
		totalQuestionsCount={vokiTakingState.totalQuestionsCount}
	/>
	<SequentialAnsweringNavigationContainer {vokiTakingState} {onResultReceived} />
{:else}
	<h1>Question error</h1>
{/if}
