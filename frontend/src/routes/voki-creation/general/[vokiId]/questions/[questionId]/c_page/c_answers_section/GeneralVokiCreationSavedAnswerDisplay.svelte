<script lang="ts">
	import type { QuestionAnswerData } from '../../../types';
	import AnswerContentViewState from './c_answer_display_contents/AnswerContentViewState.svelte';

	let {
		vokiId,
		QuestionId,
		answer
	}: { vokiId: string; QuestionId: string; answer: QuestionAnswerData } = $props<{
		vokiId: string;
		QuestionId: string;
		answer: QuestionAnswerData;
	}>();
</script>

<div class="saved-answer">
	<AnswerContentViewState answer={answer.typeData} />
	{JSON.stringify(answer)}
</div>

<style>
	.saved-answer {
		margin-top: 1.5rem;
		display: flex;
		flex-direction: column;
		align-items: center;
		justify-items: center;
		width: 100%;
		gap: 0.75rem;
		padding: 0.5rem 0.75rem;
		border: 0.125rem solid var(--secondary-foreground);
		border-radius: 0.75rem;
	}
</style>
