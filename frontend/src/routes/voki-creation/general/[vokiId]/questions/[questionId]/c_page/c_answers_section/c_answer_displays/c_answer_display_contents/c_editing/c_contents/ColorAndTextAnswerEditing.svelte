<script lang="ts">
	import type { AnswerDataColorAndText} from '$lib/ts/voki';

	let { answer = $bindable() }: { answer: AnswerDataColorAndText } = $props<{
		answer: AnswerDataColorAndText;
	}>();
</script>

<div class="answer-content">
	ColorAndText: {JSON.stringify(answer)}
</div>

<style>
	.answer-content {
		width: 100%;
	}
</style>
