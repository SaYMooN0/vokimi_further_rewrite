<script lang="ts">
	import { SequentialAnsweringGeneralVokiTakingState } from './c_sequential_answering_taking/sequential-answering-general-voki-taking-state.svelte';
	import type { GeneralVokiTakingData } from './types';

	interface Props {
		takingData: GeneralVokiTakingData;
		clearVokiSeenUpdateTimer: () => void;
	}
	let { takingData, clearVokiSeenUpdateTimer }: Props = $props();

	let vokiTakingState = new SequentialAnsweringGeneralVokiTakingState(
		takingData,
		clearVokiSeenUpdateTimer
	);
</script>

{#if vokiTakingState.currentQuestion}
	{JSON.stringify(vokiTakingState.currentQuestion)}
{:else}
	<h1>Question error</h1>
{/if}
