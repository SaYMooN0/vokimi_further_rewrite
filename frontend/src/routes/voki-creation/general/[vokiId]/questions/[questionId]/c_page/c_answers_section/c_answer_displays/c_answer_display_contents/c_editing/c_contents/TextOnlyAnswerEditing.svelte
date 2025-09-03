<script lang="ts">
	import type { AnswerDataTextOnly } from '../../../../../../types';
	import AnswerEditingTextArea from './c_shared/AnswerEditingTextArea.svelte';

	let { answer = $bindable() }: { answer: AnswerDataTextOnly } = $props<{
		answer: AnswerDataTextOnly;
	}>();
</script>

<div class="answer-content">
	<AnswerEditingTextArea bind:text={answer.text} />
</div>

<style>
	.answer-content {
		display: flex;
		flex-direction: column;
		place-items: center center;
		width: 100%;
	}
</style>
