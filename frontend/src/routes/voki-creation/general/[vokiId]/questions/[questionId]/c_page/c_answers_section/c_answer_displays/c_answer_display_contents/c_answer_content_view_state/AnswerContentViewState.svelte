<script lang="ts">
	import type { GeneralVokiAnswerTypeData } from '$lib/ts/voki';
	import IncorrectAnswerTypeMessage from '../c_answer_content_states_shared/IncorrectAnswerTypeMessage.svelte';
	import AudioAndTextAnswerView from './c_answer_content_view_states/AudioAndTextAnswerView.svelte';
	import AudioOnlyAnswerView from './c_answer_content_view_states/AudioOnlyAnswerView.svelte';
	import ColorAndTextAnswerView from './c_answer_content_view_states/ColorAndTextAnswerView.svelte';
	import ColorOnlyAnswerView from './c_answer_content_view_states/ColorOnlyAnswerView.svelte';
	import ImageAndTextAnswerView from './c_answer_content_view_states/ImageAndTextAnswerView.svelte';
	import ImageOnlyAnswerView from './c_answer_content_view_states/ImageOnlyAnswerView.svelte';
	import TextOnlyAnswerView from './c_answer_content_view_states/TextOnlyAnswerView.svelte';

	const { answer }: { answer: GeneralVokiAnswerTypeData } = $props<{
		answer: GeneralVokiAnswerTypeData;
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
