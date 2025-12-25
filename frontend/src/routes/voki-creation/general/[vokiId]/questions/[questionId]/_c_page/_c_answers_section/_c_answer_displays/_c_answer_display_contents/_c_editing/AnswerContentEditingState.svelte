<script lang="ts">
	import AudioAndTextAnswerEditing from './_c_contents/AudioAndTextAnswerEditing.svelte';
	import AudioOnlyAnswerEditing from './_c_contents/AudioOnlyAnswerEditing.svelte';
	import ColorAndTextAnswerEditing from './_c_contents/ColorAndTextAnswerEditing.svelte';
	import ColorOnlyAnswerEditing from './_c_contents/ColorOnlyAnswerEditing.svelte';
	import ImageAndTextAnswerEditing from './_c_contents/ImageAndTextAnswerEditing.svelte';
	import ImageOnlyAnswerEditing from './_c_contents/ImageOnlyAnswerEditing.svelte';
	import TextOnlyAnswerEditing from './_c_contents/TextOnlyAnswerEditing.svelte';
	import IncorrectAnswerTypeMessage from '../_c_shared/IncorrectAnswerTypeMessage.svelte';
	import type { GeneralVokiCreationAnswerData } from '../../../../../types';

	interface Props {
		answer: GeneralVokiCreationAnswerData;
	}

	let { answer = $bindable() }: Props = $props();
</script>

{#if answer.type === 'TextOnly'}
	<TextOnlyAnswerEditing bind:answer />
{:else if answer.type === 'ColorOnly'}
	<ColorOnlyAnswerEditing bind:answer />
{:else if answer.type === 'ColorAndText'}
	<ColorAndTextAnswerEditing bind:answer />
{:else if answer.type === 'ImageOnly'}
	<ImageOnlyAnswerEditing bind:answer />
{:else if answer.type === 'ImageAndText'}
	<ImageAndTextAnswerEditing bind:answer />
{:else if answer.type === 'AudioOnly'}
	<AudioOnlyAnswerEditing bind:answer />
{:else if answer.type === 'AudioAndText'}
	<AudioAndTextAnswerEditing bind:answer />
{:else}
	<IncorrectAnswerTypeMessage {answer} />
{/if}
