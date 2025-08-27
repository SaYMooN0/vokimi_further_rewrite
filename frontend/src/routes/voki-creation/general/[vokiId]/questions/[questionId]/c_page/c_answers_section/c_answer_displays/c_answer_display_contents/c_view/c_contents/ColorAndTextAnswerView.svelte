<script lang="ts">
	import type { AnswerDataColorAndText } from "../../../../../../types";


	const { answer }: { answer: AnswerDataColorAndText } = $props<{
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
