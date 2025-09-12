<script lang="ts">
	import type { AnswerDataImageOnly } from '../../../../../../types';
	import GeneralVokiCreationAnswerDisplayImage from '../../c_shared/GeneralVokiCreationAnswerDisplayImage.svelte';

	const { answer }: { answer: AnswerDataImageOnly } = $props<{
		answer: AnswerDataImageOnly;
	}>();
</script>

<div class="answer-content">
	<GeneralVokiCreationAnswerDisplayImage src={answer.image} />
</div>

<style>
	.answer-content {
		margin-inline: auto;
	}
</style>
