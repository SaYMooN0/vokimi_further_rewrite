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
		display: flex;
		flex-direction: column;
		place-items: center center;
		gap: 0.75rem;
		width: 100%;
		padding: 0.5rem 0.75rem;
		margin-top: 1rem;
		border: 0.125rem solid var(--secondary-foreground);
		border-radius: 0.75rem;
	}
</style>
