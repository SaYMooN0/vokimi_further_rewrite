<script lang="ts">
	import type { AnswerDataColorOnly } from '../../../../../../types';

	const { answer }: { answer: AnswerDataColorOnly } = $props<{
		answer: AnswerDataColorOnly;
	}>();
</script>

<div class="answer-content">
	<div class="color" style="background-color: {answer.color}"></div>
	<label>{answer.color}</label>
</div>

<style>
	.answer-content {
		width: 100%;
		height: 100%;
		display: flex;
		justify-content: center;
		align-items: center;
		flex-direction: column;
		gap: 0.5rem;
	}
	.color {
		height: 60%;
		min-height: 4rem;
		width: 90%;
		border-radius: 0.5rem;
	}
	label {
		font-size: 0.875rem;
		font-weight: 500;
		color: var(--secondary-foreground);
	}
</style>
