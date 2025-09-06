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
		display: grid;
		gap: 1rem;
		width: 100%;
		height: 100%;
		grid-template-columns: 1fr 10rem;
	}

	.color-part {
		display: flex;
		flex-direction: column;
		justify-content: center;
		align-items: center;
		gap: 0.25rem;
	}

	.color {
		width: 100%;
		height: 80%;
		min-height: 4rem;
		border-radius: 0.5rem;
	}

	.color-label {
		color: var(--secondary-foreground);
		font-size: 0.875rem;
		font-weight: 500;
	}
</style>
