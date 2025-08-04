<script lang="ts">
	import type { AnswerDataImageAndText} from '$lib/ts/voki';

	const { answer  }: { answer: AnswerDataImageAndText } = $props<{
		answer: AnswerDataImageAndText;
	}>();
</script>

<div class="answer-content">
	ImageAndText: {JSON.stringify(answer)}
</div>

<style>
	.answer-content {
		width: 100%;
	}
</style>
