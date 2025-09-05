<script lang="ts">
	import type { Err } from '$lib/ts/err';
	import type { AnswerDataImageOnly } from '../../../../../../types';
	import AnswerEditingImageInput from './c_shared/AnswerEditingImageInput.svelte';

	let { answer = $bindable() }: { answer: AnswerDataImageOnly } = $props<{
		answer: AnswerDataImageOnly;
	}>();
	let uploadingErrs = $state<Err[]>([]);
</script>

<div class="answer-content">
	<AnswerEditingImageInput
		bind:image={answer.image}
		setUploadingErrs={(errs) => (uploadingErrs = errs)}
	/>
</div>

<style>
	.answer-content {
		width: 100%;
		padding: 0 2rem;
	}
</style>
