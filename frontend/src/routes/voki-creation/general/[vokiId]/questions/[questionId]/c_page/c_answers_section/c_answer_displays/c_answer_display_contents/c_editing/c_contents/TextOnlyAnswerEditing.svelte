<script lang="ts">
	import { StringUtils } from '$lib/ts/utils/string-utils';
	import type { AnswerDataTextOnly } from '$lib/ts/voki';
	import { TextareaAutosize } from 'runed';

	let { answer = $bindable() }: { answer: AnswerDataTextOnly } = $props<{
		answer: AnswerDataTextOnly;
	}>();
	let textarea = $state<HTMLTextAreaElement>(null!);

	new TextareaAutosize({ element: () => textarea, input: () => answer.text });
</script>

<div class="answer-content">
	<textarea
		class="answer-input"
		bind:this={textarea}
		bind:value={answer.text}
		name={StringUtils.rndStr()}
	/>
</div>

<style>
	.answer-content {
		display: flex;
		flex-direction: column;
		place-items: center center;
		width: 100%;
	}

	.answer-input {
		width: 100%;
		box-sizing: border-box;
		padding: 0.25rem 0.375rem;
		border: none;
		border-radius: 0.375rem;
		background-color: var(--secondary);
		font-size: 1.125rem;
		font-weight: 500;
		outline: 0.125rem solid var(--secondary);
		resize: none;
	}

	.answer-input:hover {
		outline-color: var(--secondary-foreground);
	}

	.answer-input:focus {
		outline-color: var(--primary);
	}
</style>
