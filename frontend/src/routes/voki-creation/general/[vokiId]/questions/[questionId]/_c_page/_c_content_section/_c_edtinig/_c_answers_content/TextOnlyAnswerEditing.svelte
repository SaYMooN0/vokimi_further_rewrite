<script lang="ts">
	import { watch } from 'runed';
	import type { AnswerDataTextOnly } from '../../../../types';
	import AnswerEditingTextArea from './_c_shared/AnswerEditingTextArea.svelte';

	interface Props {
		answer: AnswerDataTextOnly;
		updateOnChange: (newAnswer: AnswerDataTextOnly) => void;
	}
	let { answer = $bindable(), updateOnChange }: Props = $props();
	let text = $state(answer.text);
	watch(
		() => text,
		() => {
			updateOnChange({ ...answer, text });
		}
	);
</script>

<div class="answer-content">
	<AnswerEditingTextArea bind:text />
</div>

<style>
	.answer-content {
		display: flex;
		flex-direction: column;
		place-items: center center;
		width: 100%;
		height: 100%;
	}
</style>
