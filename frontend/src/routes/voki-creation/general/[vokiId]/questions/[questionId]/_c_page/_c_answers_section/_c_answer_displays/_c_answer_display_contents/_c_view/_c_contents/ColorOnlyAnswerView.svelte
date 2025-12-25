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
		display: flex;
		flex-direction: column;
		justify-content: center;
		align-items: center;
		gap: 0.5rem;
		width: 100%;
		height: 100%;
	}

	.color {
		width: 90%;
		height: 60%;
		min-height: 4rem;
		border-radius: 0.5rem;
	}

	label {
		color: var(--secondary-foreground);
		font-size: 0.875rem;
		font-weight: 500;
	}
</style>
