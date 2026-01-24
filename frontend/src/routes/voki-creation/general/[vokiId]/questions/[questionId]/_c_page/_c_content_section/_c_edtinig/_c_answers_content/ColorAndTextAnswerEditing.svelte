<script lang="ts">
	import { watch } from 'runed';
	import { TextareaAutosize } from 'runed';
	import type { AnswerDataColorAndText } from '../../../../types';
	import AnswerEditingTextArea from './_c_shared/AnswerEditingTextArea.svelte';
	import AnswerEditingBasicColorInput from './_c_shared/AnswerEditingBasicColorInput.svelte';
	interface Props {
		answer: AnswerDataColorAndText;
		updateOnChange: (newAnswer: AnswerDataColorAndText) => void;
	}
	let { answer, updateOnChange }: Props = $props();
	let text = $state(answer.text);
	let color = $state(answer.color);
	watch(
		() => [text, color],
		() => {
			updateOnChange({ ...answer, text, color });
		}
	);

	let textarea = $state<HTMLTextAreaElement>(null!);
	new TextareaAutosize({ element: () => textarea, input: () => text });
</script>

<div class="answer-content">
	<AnswerEditingTextArea bind:text />
	<AnswerEditingBasicColorInput bind:color />
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
