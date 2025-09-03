<script lang="ts">
	import { StringUtils } from '$lib/ts/utils/string-utils';
	import { TextareaAutosize } from 'runed';
	import type { AnswerDataColorAndText } from '../../../../../../types';
	import AnswerEditingColorInput from './c_shared/AnswerEditingColorInput.svelte';
	import AnswerEditingTextArea from './c_shared/AnswerEditingTextArea.svelte';

	let { answer = $bindable() }: { answer: AnswerDataColorAndText } = $props<{
		answer: AnswerDataColorAndText;
	}>();

	let textarea = $state<HTMLTextAreaElement>(null!);
	new TextareaAutosize({ element: () => textarea, input: () => answer.text });
</script>

<div class="answer-content">
	<AnswerEditingColorInput bind:color={answer.color} />
	<AnswerEditingTextArea bind:text={answer.text} />
</div>

<style>
	.answer-content {
		width: 100%;
		display: grid;
		grid-template-columns: auto 1fr;
		gap: 1rem;
		padding: 0.5rem 0 0;
	}
</style>
