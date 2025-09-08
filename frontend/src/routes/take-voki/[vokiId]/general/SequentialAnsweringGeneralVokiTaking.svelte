<script lang="ts">
	import { SequentialAnsweringGeneralVokiTakingState } from './c_sequential_answering_taking/sequential-answering-general-voki-taking-state.svelte';
	import GeneralVokiReceivedResultView from './c_takings_shared/GeneralVokiReceivedResultView.svelte';
	import type { GeneralVokiTakingData, GeneralVokiTakingResultData } from './types';

	let { takingData }: { takingData: GeneralVokiTakingData } = $props<{
		takingData: GeneralVokiTakingData;
	}>();
	let vokiTakingState = new SequentialAnsweringGeneralVokiTakingState(takingData);
	let receivedResult: GeneralVokiTakingResultData | null = $state(null);

</script>

{#if receivedResult === null}
	{#if vokiTakingState.currentQuestion}
		{JSON.stringify(vokiTakingState.currentQuestion)}
		
	{:else}
		<h1>Question error</h1>
	{/if}
{:else}
	<GeneralVokiReceivedResultView
		result={receivedResult}
		allowToSeeResultsList={true}
		vokiId={vokiTakingState.vokiId}
	/>
{/if}
