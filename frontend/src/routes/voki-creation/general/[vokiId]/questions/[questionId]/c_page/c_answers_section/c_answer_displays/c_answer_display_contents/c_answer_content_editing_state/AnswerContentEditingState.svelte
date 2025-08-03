<script lang="ts">
	import type { GeneralVokiAnswerTypeData } from '$lib/ts/voki';
	import AudioAndTextAnswerEditing from './c_answer_content_editing_states/AudioAndTextAnswerEditing.svelte';
	import AudioOnlyAnswerEditing from './c_answer_content_editing_states/AudioOnlyAnswerEditing.svelte';
	import ColorAndTextAnswerEditing from './c_answer_content_editing_states/ColorAndTextAnswerEditing.svelte';
	import ColorOnlyAnswerEditing from './c_answer_content_editing_states/ColorOnlyAnswerEditing.svelte';
	import ImageAndTextAnswerEditing from './c_answer_content_editing_states/ImageAndTextAnswerEditing.svelte';
	import ImageOnlyAnswerEditing from './c_answer_content_editing_states/ImageOnlyAnswerEditing.svelte';
	import TextOnlyAnswerEditing from './c_answer_content_editing_states/TextOnlyAnswerEditing.svelte';
	import IncorrectAnswerTypeMessage from '../c_answer_content_states_shared/IncorrectAnswerTypeMessage.svelte';

	let { answer = $bindable() }: { answer: GeneralVokiAnswerTypeData } = $props<{
		answer: GeneralVokiAnswerTypeData;
	}>();
</script>

{#if answer.type === 'TextOnly'}
	<TextOnlyAnswerEditing bind:answer />
{:else if answer.type === 'ImageOnly'}
	<ImageOnlyAnswerEditing bind:answer />
{:else if answer.type === 'ImageAndText'}
	<ImageAndTextAnswerEditing bind:answer />
{:else if answer.type === 'ColorOnly'}
	<ColorOnlyAnswerEditing bind:answer />
{:else if answer.type === 'ColorAndText'}
	<ColorAndTextAnswerEditing bind:answer />
{:else if answer.type === 'AudioOnly'}
	<AudioOnlyAnswerEditing bind:answer />
{:else if answer.type === 'AudioAndText'}
	<AudioAndTextAnswerEditing bind:answer />
{:else}
	<IncorrectAnswerTypeMessage {answer} />
{/if}
