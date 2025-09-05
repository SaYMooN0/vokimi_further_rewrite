<script lang="ts">
	import type { AnswerDataColorAndText } from '../../../../../../types';
	import AnswersViewTextDisplay from './c_shared/AnswersViewTextDisplay.svelte';

	const { answer }: { answer: AnswerDataColorAndText } = $props<{
		answer: AnswerDataColorAndText;
	}>();
</script>

<div class="answer-content">
	<AnswersViewTextDisplay text={answer.text} />
	<div class="color-part">
		<div class="color" style="background-color: {answer.color}"></div>
		<label class="color-label">{answer.color}</label>
	</div>
</div>

<style>
	.answer-content {
		width: 100%;
		display: grid;
		grid-template-columns: 1fr 10rem;
		gap: 1rem;
		height: 100%;
	}
	.color-part {
		display: flex;
		justify-content: center;
		flex-direction: column;
		align-items: center;
		gap: 0.25rem;
	}
	.color {
		height: 80%;
		min-height: 4rem;
		width: 100%;
		border-radius: 0.5rem;
	}
	.color-label {
		font-size: 0.875rem;
		font-weight: 500;
		color: var(--secondary-foreground);
	}
</style>
