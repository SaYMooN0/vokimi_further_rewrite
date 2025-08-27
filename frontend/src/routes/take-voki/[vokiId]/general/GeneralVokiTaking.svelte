<script lang="ts">
	import { GeneralVokiTakingState } from './general-voki-taking-state.svelte';
	import type { GeneralVokiTakingData } from './types';

	let { takingData }: { takingData: GeneralVokiTakingData } = $props<{ takingData: GeneralVokiTakingData }>();
	let pageState = new GeneralVokiTakingState(voki);
</script>

<div>{pageState}</div>
