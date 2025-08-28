<script lang="ts">
	import { SequentialAnsweringGeneralVokiTakingState } from './c_sequential_answering_taking/sequential-answering-general-voki-taking-state.svelte';
	import type { GeneralVokiTakingData } from './types';

	let { takingData }: { takingData: GeneralVokiTakingData } = $props<{
		takingData: GeneralVokiTakingData;
	}>();
	let vokiTakingState = new SequentialAnsweringGeneralVokiTakingState(takingData);
</script>

<h1>Is not implemented yet</h1>
