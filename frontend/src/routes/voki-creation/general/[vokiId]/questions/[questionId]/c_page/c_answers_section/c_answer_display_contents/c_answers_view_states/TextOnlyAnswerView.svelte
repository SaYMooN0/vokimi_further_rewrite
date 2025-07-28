<script lang="ts">
	import type { AnswerDataTextOnly } from '$lib/ts/voki';

	let { answer }: { answer: AnswerDataTextOnly } = $props<{
		answer: AnswerDataTextOnly;
	}>();
</script>

<div class="answer-content">
	<p>{answer.text}</p>
</div>
