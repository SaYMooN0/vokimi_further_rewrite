<script lang="ts">
	import type { AnswerDataColorOnly } from "../../../../../../types";


	let { answer = $bindable() }: { answer: AnswerDataColorOnly } = $props<{
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
