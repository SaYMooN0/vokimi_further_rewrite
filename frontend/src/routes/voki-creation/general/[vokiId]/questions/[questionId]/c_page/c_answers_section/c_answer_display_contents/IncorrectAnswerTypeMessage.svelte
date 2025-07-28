<script lang="ts">
	const { answerType }: { answerType?: string } = $props<{
		answerType?: string;
	}>();
</script>

<div class="message-container">
	{#if answerType}
		<p>Unexpected answer type: {answerType}</p>
	{:else}
		<p>Answer type is not provided</p>
	{/if}
</div>
