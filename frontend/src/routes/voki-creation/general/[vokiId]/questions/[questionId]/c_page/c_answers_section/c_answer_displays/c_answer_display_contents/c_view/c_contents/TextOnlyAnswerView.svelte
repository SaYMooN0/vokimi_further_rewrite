<script lang="ts">
	import type { AnswerDataTextOnly } from '$lib/ts/voki';

	const { answer }: { answer: AnswerDataTextOnly } = $props<{
		answer: AnswerDataTextOnly;
	}>();
</script>

<div class="answer-content">
	<p>{answer.text}</p>
</div>

<style>
	.answer-content {
		display: flex;
		flex-direction: column;
		place-items: center center;
		width: 100%;
	}

	p {
		width: 100%;
		box-sizing: border-box;
		font-size: 1.125rem;
		font-weight: 500;
				word-break: break-all;

	}
</style>
