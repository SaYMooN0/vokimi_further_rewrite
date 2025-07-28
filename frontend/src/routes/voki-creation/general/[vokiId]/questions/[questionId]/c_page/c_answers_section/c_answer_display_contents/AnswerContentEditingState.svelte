<script lang="ts">
	import type { GeneralVokiAnswerTypeData } from '$lib/ts/voki';
	import TextOnlyAnswerEditing from './c_answers_editing_states/TextOnlyAnswerEditing.svelte';
	import IncorrectAnswerTypeMessage from './IncorrectAnswerTypeMessage.svelte';

	let { answer }: { answer: GeneralVokiAnswerTypeData } = $props<{
		answer: GeneralVokiAnswerTypeData;
	}>();
</script>

{#if answer.answerType === 'TextOnly'}
	<TextOnlyAnswerEditing {answer} />
{:else}
	<IncorrectAnswerTypeMessage answerType={answer.answerType} />
{/if}
