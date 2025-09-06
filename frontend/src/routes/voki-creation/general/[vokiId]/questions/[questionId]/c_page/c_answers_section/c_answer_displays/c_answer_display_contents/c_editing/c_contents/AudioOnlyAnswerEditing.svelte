<script lang="ts">
	import type { AnswerDataAudioOnly } from '../../../../../../types';
	interface Props {
		answer: AnswerDataAudioOnly;
		vokiId: string;
		questionId: string;
	}
	let { answer = $bindable(), vokiId, questionId }: Props = $props();
</script>

<div class="answer-content">
	AudioOnly: {JSON.stringify(answer)}
</div>

<style>
	.answer-content {
		width: 100%;
	}
</style>
