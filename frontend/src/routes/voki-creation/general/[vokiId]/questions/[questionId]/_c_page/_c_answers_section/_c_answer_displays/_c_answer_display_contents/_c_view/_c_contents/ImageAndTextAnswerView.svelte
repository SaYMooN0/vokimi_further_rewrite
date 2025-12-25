<script lang="ts">
	import type { AnswerDataImageAndText } from '../../../../../../types';
	import GeneralVokiCreationAnswerDisplayImage from '../../_c_shared/GeneralVokiCreationAnswerDisplayImage.svelte';
	import AnswersViewTextDisplay from './_c_shared/AnswersViewTextDisplay.svelte';

	const { answer }: { answer: AnswerDataImageAndText } = $props<{
		answer: AnswerDataImageAndText;
	}>();
</script>

<div class="answer-content">
	<AnswersViewTextDisplay text={answer.text} />
	<GeneralVokiCreationAnswerDisplayImage src={answer.image} maxWidth={24} maxHeight={14} />
</div>

<style>
	.answer-content {
		display: grid;
		grid-template-columns: 1fr auto;
		gap: 0.5rem;
		margin-inline: auto;
		width: 100%;
	}
</style>
