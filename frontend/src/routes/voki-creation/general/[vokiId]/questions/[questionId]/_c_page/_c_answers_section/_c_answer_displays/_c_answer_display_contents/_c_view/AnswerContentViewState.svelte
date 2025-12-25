<script lang="ts">
	import type { GeneralVokiCreationAnswerData } from '../../../../../types';
	import IncorrectAnswerTypeMessage from '../_c_shared/IncorrectAnswerTypeMessage.svelte';
	import AudioAndTextAnswerView from './_c_contents/AudioAndTextAnswerView.svelte';
	import AudioOnlyAnswerView from './_c_contents/AudioOnlyAnswerView.svelte';
	import ColorAndTextAnswerView from './_c_contents/ColorAndTextAnswerView.svelte';
	import ColorOnlyAnswerView from './_c_contents/ColorOnlyAnswerView.svelte';
	import ImageAndTextAnswerView from './_c_contents/ImageAndTextAnswerView.svelte';
	import ImageOnlyAnswerView from './_c_contents/ImageOnlyAnswerView.svelte';
	import TextOnlyAnswerView from './_c_contents/TextOnlyAnswerView.svelte';

	const { answer }: { answer: GeneralVokiCreationAnswerData } = $props<{
		answer: GeneralVokiCreationAnswerData;
	}>();
</script>

{#if answer.type === 'TextOnly'}
	<TextOnlyAnswerView {answer} />
{:else if answer.type === 'ImageOnly'}
	<ImageOnlyAnswerView {answer} />
{:else if answer.type === 'ImageAndText'}
	<ImageAndTextAnswerView {answer} />
{:else if answer.type === 'ColorOnly'}
	<ColorOnlyAnswerView {answer} />
{:else if answer.type === 'ColorAndText'}
	<ColorAndTextAnswerView {answer} />
{:else if answer.type === 'AudioOnly'}
	<AudioOnlyAnswerView {answer} />
{:else if answer.type === 'AudioAndText'}
	<AudioAndTextAnswerView {answer} />
{:else}
	<IncorrectAnswerTypeMessage {answer} />
{/if}
