<script lang="ts">
	import { DefaultGeneralVokiTakingState } from './c_default_taking/default-general-voki-taking-state.svelte';
	import DefaultTakingButtonsContainer from './c_default_taking/DefaultTakingButtonsContainer.svelte';
	import GeneralVokiReceivedResultView from './c_takings_shared/GeneralVokiReceivedResultView.svelte';
	import type { GeneralVokiTakingData } from './types';

	let { takingData }: { takingData: GeneralVokiTakingData } = $props<{
		takingData: GeneralVokiTakingData;
	}>();
	let vokiTakingState = new DefaultGeneralVokiTakingState(takingData);
</script>

<div>{vokiTakingState}</div>
{#if vokiTakingState.receivedResult === null}
	<DefaultTakingButtonsContainer {vokiTakingState} />
{:else}
	<GeneralVokiReceivedResultView
		result={vokiTakingState.receivedResult}
		allowToSeeResultsList={true}
		vokiId={vokiTakingState.vokiId}
	/>
{/if}
