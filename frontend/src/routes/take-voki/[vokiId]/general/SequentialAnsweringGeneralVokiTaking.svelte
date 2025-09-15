<script lang="ts">
	import { SequentialAnsweringGeneralVokiTakingState } from './c_sequential_answering_taking/sequential-answering-general-voki-taking-state.svelte';
	import GeneralVokiReceivedResultView from './c_takings_shared/GeneralVokiReceivedResultView.svelte';
	import type { GeneralVokiTakenResult, GeneralVokiTakingData } from './types';

	interface Props {
		takingData: GeneralVokiTakingData;
		clearVokiSeenUpdateTimer: () => void;
	}
	let { takingData, clearVokiSeenUpdateTimer }: Props = $props();

	let vokiTakingState = new SequentialAnsweringGeneralVokiTakingState(
		takingData,
		clearVokiSeenUpdateTimer
	);
	let vokiTakenData: GeneralVokiTakenResult | null = $state(null);
</script>

{#if vokiTakenData === null}
	{#if vokiTakingState.currentQuestion}
		{JSON.stringify(vokiTakingState.currentQuestion)}
	{:else}
		<h1>Question error</h1>
	{/if}
{:else}
	<GeneralVokiReceivedResultView
		result={vokiTakenData.receivedResult}
		allowToSeeResultsList={true}
		vokiId={vokiTakingState.vokiId}
	/>
{/if}
