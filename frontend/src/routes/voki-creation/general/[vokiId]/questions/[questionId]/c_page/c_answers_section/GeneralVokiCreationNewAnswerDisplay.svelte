<script lang="ts">
	import type { GeneralVokiAnswerTypeData } from '$lib/ts/voki';
	import AnswerContentEditingState from './c_answer_display_contents/AnswerContentEditingState.svelte';

	let {
		vokiId,
		QuestionId,
		answer
	}: { vokiId: string; QuestionId: string; answer: GeneralVokiAnswerTypeData } = $props<{
		vokiId: string;
		QuestionId: string;
		answer: GeneralVokiAnswerTypeData;
	}>();
</script>

<div class="unsaved-answer">
	<AnswerContentEditingState {answer} />
	<div class="buttons-container">
		<button>Save</button>
		<button>Cancel</button>
	</div>
</div>

<style>
	.unsaved-answer {
        margin-top: 1.5rem;
		display: flex;
		flex-direction: column;
		align-items: center;
		justify-items: center;
		width: 100%;
	}
	.buttons-container {
	}
</style>
