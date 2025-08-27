<script lang="ts">
	import type { AnswerDataAudioAndText } from '../../../../../../types';

	const { answer }: { answer: AnswerDataAudioAndText } = $props<{
		answer: AnswerDataAudioAndText;
	}>();
</script>

<div class="answer-content">
	AudioAndText: {JSON.stringify(answer)}
</div>

<style>
	.answer-content {
		width: 100%;
	}
</style>
