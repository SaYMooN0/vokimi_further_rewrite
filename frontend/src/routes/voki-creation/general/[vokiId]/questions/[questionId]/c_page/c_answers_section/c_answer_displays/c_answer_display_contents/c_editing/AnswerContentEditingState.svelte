<script lang="ts">
	import type { GeneralVokiAnswerTypeData } from '$lib/ts/voki';
	import AudioAndTextAnswerEditing from './c_contents/AudioAndTextAnswerEditing.svelte';
	import AudioOnlyAnswerEditing from './c_contents/AudioOnlyAnswerEditing.svelte';
	import ColorAndTextAnswerEditing from './c_contents/ColorAndTextAnswerEditing.svelte';
	import ColorOnlyAnswerEditing from './c_contents/ColorOnlyAnswerEditing.svelte';
	import ImageAndTextAnswerEditing from './c_contents/ImageAndTextAnswerEditing.svelte';
	import ImageOnlyAnswerEditing from './c_contents/ImageOnlyAnswerEditing.svelte';
	import TextOnlyAnswerEditing from './c_contents/TextOnlyAnswerEditing.svelte';
	import IncorrectAnswerTypeMessage from '../c_shared/IncorrectAnswerTypeMessage.svelte';

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
