<script lang="ts">
	import type { AnswerDataColorOnly} from '$lib/ts/voki';

	const { answer }: { answer: AnswerDataColorOnly } = $props<{
		answer: AnswerDataColorOnly;
	}>();
</script>

<div class="answer-content">
	ColorOnly: {JSON.stringify(answer)}
</div>

<style>
	.answer-content {
		width: 100%;
	}
</style>
