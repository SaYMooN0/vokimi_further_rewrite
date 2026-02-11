<script lang="ts">
	import { SequentialAnsweringGeneralVokiTakingState } from './_c_sequential_answering_taking/sequential-answering-general-voki-taking-state.svelte';
	import SequentialAnsweringCurrentQuestionView from './_c_sequential_answering_taking/SequentialAnsweringCurrentQuestionView.svelte';
	import SequentialAnsweringNavigationContainer from './_c_sequential_answering_taking/SequentialAnsweringNavigationContainer.svelte';
	import type { GeneralVokiTakingData, PosssibleGeneralVokiTakingDataSaveData } from './types';

	interface Props {
		takingData: GeneralVokiTakingData;
		saveData: PosssibleGeneralVokiTakingDataSaveData;
		clearVokiSeenUpdateTimer: () => void;
		onResultReceived: (resultId: string) => void;
	}
	let { takingData, saveData, clearVokiSeenUpdateTimer, onResultReceived }: Props = $props();

	let vokiTakingState = new SequentialAnsweringGeneralVokiTakingState(
		takingData,
		saveData,
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
