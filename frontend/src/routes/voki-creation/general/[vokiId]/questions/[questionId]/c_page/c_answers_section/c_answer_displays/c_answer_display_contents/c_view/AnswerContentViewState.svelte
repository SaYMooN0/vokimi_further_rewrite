<script lang="ts">
	import type { GeneralVokiCreationAnswerData } from '../../../../../types';
	import IncorrectAnswerTypeMessage from '../c_shared/IncorrectAnswerTypeMessage.svelte';
	import AudioAndTextAnswerView from './c_contents/AudioAndTextAnswerView.svelte';
	import AudioOnlyAnswerView from './c_contents/AudioOnlyAnswerView.svelte';
	import ColorAndTextAnswerView from './c_contents/ColorAndTextAnswerView.svelte';
	import ColorOnlyAnswerView from './c_contents/ColorOnlyAnswerView.svelte';
	import ImageAndTextAnswerView from './c_contents/ImageAndTextAnswerView.svelte';
	import ImageOnlyAnswerView from './c_contents/ImageOnlyAnswerView.svelte';
	import TextOnlyAnswerView from './c_contents/TextOnlyAnswerView.svelte';

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
