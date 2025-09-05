<script lang="ts">
	import { TextareaAutosize } from 'runed';
	import type { AnswerDataColorAndText } from '../../../../../../types';
	import AnswerEditingTextArea from './c_shared/AnswerEditingTextArea.svelte';
	import AnswerEditingBasicColorInput from './c_shared/AnswerEditingBasicColorInput.svelte';

	let { answer = $bindable() }: { answer: AnswerDataColorAndText } = $props<{
		answer: AnswerDataColorAndText;
	}>();

	let textarea = $state<HTMLTextAreaElement>(null!);
	new TextareaAutosize({ element: () => textarea, input: () => answer.text });
</script>

<div class="answer-content">
	<AnswerEditingTextArea bind:text={answer.text} />
	<AnswerEditingBasicColorInput bind:color={answer.color} />
</div>

<style>
	.answer-content {
		width: 100%;
		display: grid;
		grid-template-columns: 1fr auto;
		gap: 1rem;
		padding: 0.5rem 0 0;
		height: 100%;
		align-items: center;

	}
</style>
