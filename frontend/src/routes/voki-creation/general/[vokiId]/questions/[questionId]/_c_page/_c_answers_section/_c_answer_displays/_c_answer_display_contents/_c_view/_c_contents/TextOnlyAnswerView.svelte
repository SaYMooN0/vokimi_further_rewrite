<script lang="ts">
	import type { AnswerDataTextOnly } from '../../../../../../types';
	import AnswersViewTextDisplay from './_c_shared/AnswersViewTextDisplay.svelte';

	const { answer }: { answer: AnswerDataTextOnly } = $props<{
		answer: AnswerDataTextOnly;
	}>();
</script>

<div class="answer-content">
	<AnswersViewTextDisplay text={answer.text} />
</div>

<style>
	.answer-content {
		display: flex;
		flex-direction: column;
		place-items: center center;
		width: 100%;
	}
</style>
