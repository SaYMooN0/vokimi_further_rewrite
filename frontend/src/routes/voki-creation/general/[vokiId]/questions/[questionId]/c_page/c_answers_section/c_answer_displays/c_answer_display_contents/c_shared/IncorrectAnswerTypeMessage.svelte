<script lang="ts">
	import type { GeneralVokiCreationAnswerData } from "../../../../../types";


	const { answer }: { answer?: GeneralVokiCreationAnswerData } = $props<{
		answer?: GeneralVokiCreationAnswerData;
	}>();
</script>

<div class="message-container">
	{#if answer && answer}
		<p>Unexpected answer type: {answer.type}</p>
	{:else}
		<p>Answer or answer type is not provided</p>
	{/if}
</div>
