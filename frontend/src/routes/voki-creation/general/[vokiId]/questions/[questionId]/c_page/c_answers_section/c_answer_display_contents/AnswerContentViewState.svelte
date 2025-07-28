<script lang="ts">
	import type { GeneralVokiAnswerTypeData } from '$lib/ts/voki';
	import ImageAndTextAnswerView from './c_answers_view_states/ImageAndTextAnswerView.svelte';
	import TextOnlyAnswerView from './c_answers_view_states/TextOnlyAnswerView.svelte';

	let { answer }: { answer: GeneralVokiAnswerTypeData } = $props<{
		answer: GeneralVokiAnswerTypeData;
	}>();
</script>

{#if answer.answerType === 'TextOnly'}
	<TextOnlyAnswerView {answer} />
{:else if answer.answerType === 'ImageAndText'}
	<ImageAndTextAnswerView {answer} />
{/if}
