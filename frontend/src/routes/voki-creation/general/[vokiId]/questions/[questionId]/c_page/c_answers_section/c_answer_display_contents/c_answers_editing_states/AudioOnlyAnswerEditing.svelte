<script lang="ts">
	import type { AnswerDataAudioOnly} from '$lib/ts/voki';

	let { answer = $bindable() }: { answer: AnswerDataAudioOnly } = $props<{
		answer: AnswerDataAudioOnly;
	}>();
</script>

<div class="answer-content">
	AudioOnly: {JSON.stringify(answer)}
</div>

<style>
	.answer-content {
		width: 100%;
	}
</style>
