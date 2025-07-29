<script lang="ts">
	import type { GeneralVokiAnswerTypeData } from '$lib/ts/voki';

	const { answer }: { answer?: GeneralVokiAnswerTypeData } = $props<{
		answer?: GeneralVokiAnswerTypeData;
	}>();
</script>

<div class="message-container">
	{#if answer && answer.answerType}
		<p>Unexpected answer type: {answer.answerType}</p>
	{:else}
		<p>Answer or answer type is not provided</p>
	{/if}
</div>
