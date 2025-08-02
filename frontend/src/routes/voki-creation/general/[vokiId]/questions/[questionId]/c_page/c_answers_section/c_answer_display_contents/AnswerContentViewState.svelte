<script lang="ts">
	import type { GeneralVokiAnswerTypeData } from '$lib/ts/voki';
	import AudioAndTextAnswerView from './c_answers_view_states/AudioAndTextAnswerView.svelte';
	import AudioOnlyAnswerView from './c_answers_view_states/AudioOnlyAnswerView.svelte';
	import ColorAndTextAnswerView from './c_answers_view_states/ColorAndTextAnswerView.svelte';
	import ColorOnlyAnswerView from './c_answers_view_states/ColorOnlyAnswerView.svelte';
	import ImageAndTextAnswerView from './c_answers_view_states/ImageAndTextAnswerView.svelte';
	import ImageOnlyAnswerView from './c_answers_view_states/ImageOnlyAnswerView.svelte';
	import TextOnlyAnswerView from './c_answers_view_states/TextOnlyAnswerView.svelte';
	import IncorrectAnswerTypeMessage from './IncorrectAnswerTypeMessage.svelte';

	const { answer }: { answer: GeneralVokiAnswerTypeData } = $props<{
		answer: GeneralVokiAnswerTypeData;
	}>();
</script>

<p>"type:" {answer.type}</p>
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
