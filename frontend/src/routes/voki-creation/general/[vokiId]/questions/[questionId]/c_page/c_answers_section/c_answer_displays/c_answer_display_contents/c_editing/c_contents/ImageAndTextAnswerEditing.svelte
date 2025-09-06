<script lang="ts">
	import DefaultErrBlock from '$lib/components/errs/DefaultErrBlock.svelte';
	import type { Err } from '$lib/ts/err';
	import type { AnswerDataImageAndText } from '../../../../../../types';
	import AnswerEditingImageInput from './c_shared/AnswerEditingImageInput.svelte';
	import AnswerEditingTextArea from './c_shared/AnswerEditingTextArea.svelte';

	interface Props {
		answer: AnswerDataImageAndText;
		vokiId: string;
		questionId: string;
	}
	let { answer = $bindable(), vokiId, questionId }: Props = $props();
	let uploadingErrs = $state<Err[]>([]);
</script>

<div class="answer-content">
	<div class="main">
		<AnswerEditingTextArea bind:text={answer.text} />
		<AnswerEditingImageInput
			bind:image={answer.image}
			setUploadingErrs={(errs) => (uploadingErrs = errs)}
			{vokiId}
			{questionId}
		/>
	</div>
	{#if uploadingErrs.length > 0}
		<DefaultErrBlock errList={uploadingErrs} />
	{/if}
</div>

<style>
	.answer-content {
		width: 100%;
		height: 100%;
		padding: 0 2rem;
		display: flex;
		align-items: center;
		flex-direction: column;
	}
	.main {
		display: grid;
		grid-template-columns: 1fr auto;
		gap: 1rem;
		width: 100%;
		height: 100%;
	}

	.answer-content > :global(.err-block) {
		width: 100%;
		margin-top: 0.5rem;
	}
</style>
