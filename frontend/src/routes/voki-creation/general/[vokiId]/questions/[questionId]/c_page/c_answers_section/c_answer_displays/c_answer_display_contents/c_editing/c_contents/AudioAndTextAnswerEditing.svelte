<script lang="ts">
	import type { AnswerDataAudioAndText} from '$lib/ts/voki';

	let { answer = $bindable() }: { answer: AnswerDataAudioAndText } = $props<{
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
