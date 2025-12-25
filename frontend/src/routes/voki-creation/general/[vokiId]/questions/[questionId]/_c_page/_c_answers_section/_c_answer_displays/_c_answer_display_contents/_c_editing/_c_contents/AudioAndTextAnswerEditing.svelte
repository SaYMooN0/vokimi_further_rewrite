<script lang="ts">
	import type { AnswerDataAudioAndText } from '../../../../../../types';

	interface Props {
		answer: AnswerDataAudioAndText;
	}
	let { answer = $bindable() }: Props = $props();
</script>

<div class="answer-content">
	AudioAndText: {JSON.stringify(answer)}
</div>

<style>
	.answer-content {
		width: 100%;
	}
</style>
