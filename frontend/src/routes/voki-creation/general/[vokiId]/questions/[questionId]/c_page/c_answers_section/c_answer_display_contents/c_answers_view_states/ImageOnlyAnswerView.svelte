<script lang="ts">
	import type { AnswerDataImageOnly } from '$lib/ts/voki';

	const { answer }: { answer: AnswerDataImageOnly } = $props<{
		answer: AnswerDataImageOnly;
	}>();
</script>

<div class="answer-content">
	Image only: {JSON.stringify(answer)}
</div>

<style>
	.answer-content {
		width: 100%;
	}
</style>
