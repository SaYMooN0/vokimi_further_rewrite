<script lang="ts">
	import type { AnswerDataAudioOnly } from '../../../../../../types';
	interface Props {
		answer: AnswerDataAudioOnly;
	}
	let { answer = $bindable() }: Props = $props();
</script>

<div class="answer-content">
	AudioOnly: {JSON.stringify(answer)}
</div>

<style>
	.answer-content {
		width: 100%;
	}
</style>
