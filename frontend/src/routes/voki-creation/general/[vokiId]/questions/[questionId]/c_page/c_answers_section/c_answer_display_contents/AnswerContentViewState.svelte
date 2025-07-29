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

{#if answer.answerType === 'TextOnly'}
	<TextOnlyAnswerView {answer} />
{:else if answer.answerType === 'ImageOnly'}
	<ImageOnlyAnswerView {answer} />
{:else if answer.answerType === 'ImageAndText'}
	<ImageAndTextAnswerView {answer} />
{:else if answer.answerType === 'ColorOnly'}
	<ColorOnlyAnswerView {answer} />
{:else if answer.answerType === 'ColorAndText'}
	<ColorAndTextAnswerView {answer} />
{:else if answer.answerType === 'AudioOnly'}
	<AudioOnlyAnswerView {answer} />
{:else if answer.answerType === 'AudioAndText'}
	<AudioAndTextAnswerView {answer} />
{:else}
	<IncorrectAnswerTypeMessage {answer} />
{/if}
