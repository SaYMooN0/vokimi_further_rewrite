<script lang="ts">
	import type { AnswerDataImageAndText } from "../../../../../../types";


	let { answer = $bindable() }: { answer: AnswerDataImageAndText } = $props<{
		answer: AnswerDataImageAndText;
	}>();
</script>

<div class="answer-content">
	ImageAndText: {JSON.stringify(answer)}
</div>

<style>
	.answer-content {
		width: 100%;
	}
</style>
