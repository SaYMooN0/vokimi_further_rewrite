<script lang="ts">
	import type { AnswerDataImageAndText } from '$lib/ts/voki';

	let { answer }: { answer: AnswerDataImageAndText } = $props<{
		answer: AnswerDataImageAndText;
	}>();
</script>

<div class="answer-content">
	<p>{answer.text}</p>
	<p>{answer.image}</p>
</div>
