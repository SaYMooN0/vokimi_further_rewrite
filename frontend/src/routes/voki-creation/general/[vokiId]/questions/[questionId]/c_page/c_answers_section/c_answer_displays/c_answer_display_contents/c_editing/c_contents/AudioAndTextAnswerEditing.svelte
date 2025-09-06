<script lang="ts">
	import type { AnswerDataAudioAndText } from '../../../../../../types';

	interface Props {
		answer: AnswerDataAudioAndText;
		vokiId: string;
		questionId: string;
	}
	let { answer = $bindable(), vokiId, questionId }: Props = $props();
</script>

<div class="answer-content">
	AudioAndText: {JSON.stringify(answer)}
</div>

<style>
	.answer-content {
		width: 100%;
	}
</style>
