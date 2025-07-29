<script lang="ts">
	import type { GeneralVokiAnswerTypeData } from '$lib/ts/voki';
	import AudioAndTextAnswerEditing from './c_answers_editing_states/AudioAndTextAnswerEditing.svelte';
	import AudioOnlyAnswerEditing from './c_answers_editing_states/AudioOnlyAnswerEditing.svelte';
	import ColorAndTextAnswerEditing from './c_answers_editing_states/ColorAndTextAnswerEditing.svelte';
	import ColorOnlyAnswerEditing from './c_answers_editing_states/ColorOnlyAnswerEditing.svelte';
	import ImageAndTextAnswerEditing from './c_answers_editing_states/ImageAndTextAnswerEditing.svelte';
	import ImageOnlyAnswerEditing from './c_answers_editing_states/ImageOnlyAnswerEditing.svelte';
	import TextOnlyAnswerEditing from './c_answers_editing_states/TextOnlyAnswerEditing.svelte';
	import IncorrectAnswerTypeMessage from './IncorrectAnswerTypeMessage.svelte';

	let { answer = $bindable() }: { answer: GeneralVokiAnswerTypeData } = $props<{
		answer: GeneralVokiAnswerTypeData;
	}>();
</script>

{#if answer.answerType === 'TextOnly'}
	<TextOnlyAnswerEditing {answer} />
{:else if answer.answerType === 'ImageOnly'}
	<ImageOnlyAnswerEditing {answer} />
{:else if answer.answerType === 'ImageAndText'}
	<ImageAndTextAnswerEditing {answer} />
{:else if answer.answerType === 'ColorOnly'}
	<ColorOnlyAnswerEditing {answer} />
{:else if answer.answerType === 'ColorAndText'}
	<ColorAndTextAnswerEditing {answer} />
{:else if answer.answerType === 'AudioOnly'}
	<AudioOnlyAnswerEditing {answer} />
{:else if answer.answerType === 'AudioAndText'}
	<AudioAndTextAnswerEditing {answer} />
{:else}
	<IncorrectAnswerTypeMessage {answer} />
{/if}
