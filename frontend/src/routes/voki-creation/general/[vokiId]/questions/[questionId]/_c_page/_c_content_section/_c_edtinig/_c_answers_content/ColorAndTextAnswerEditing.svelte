<script lang="ts">
	import { TextareaAutosize } from 'runed';
	import type { AnswerDataColorAndText } from '../../../../types';
	import AnswerEditingTextArea from './_c_shared/AnswerEditingTextArea.svelte';
	import AnswerEditingBasicColorInput from './_c_shared/AnswerEditingBasicColorInput.svelte';
	interface Props {
		answer: AnswerDataColorAndText;
	}
	let { answer = $bindable() }: Props = $props();

	let textarea = $state<HTMLTextAreaElement>(null!);
	new TextareaAutosize({ element: () => textarea, input: () => answer.text });
</script>

<div class="answer-content">
	<AnswerEditingTextArea bind:text={answer.text} />
	<AnswerEditingBasicColorInput bind:color={answer.color} />
</div>

<style>
	.answer-content {
		display: grid;
		align-items: center;
		gap: 1rem;
		width: 100%;
		height: 100%;
		padding: 0.5rem 0 0;
		grid-template-columns: 1fr auto;
	}
</style>
