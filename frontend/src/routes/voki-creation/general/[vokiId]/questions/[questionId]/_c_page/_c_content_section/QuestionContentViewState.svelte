<script lang="ts">
	import VokiCreationDefaultButton from '../../../../../../_c_shared/VokiCreationDefaultButton.svelte';
	import type { GeneralVokiCreationQuestionContent } from '../../types';
	import type { QuestionPageResultsState } from '../../general-voki-creation-specific-question-page-state.svelte';
	import IncorrectContentTypeMessage from './_c_shared/IncorrectContentTypeMessage.svelte';
	import QuestionTextOnlyContentView from './_c_view/QuestionTextOnlyContentView.svelte';
	import QuestionImageOnlyContentView from './_c_view/QuestionImageOnlyContentView.svelte';
	import QuestionImageAndTextContentView from './_c_view/QuestionImageAndTextContentView.svelte';
	import QuestionColorOnlyContentView from './_c_view/QuestionColorOnlyContentView.svelte';
	import QuestionColorAndTextContentView from './_c_view/QuestionColorAndTextContentView.svelte';

	interface Props {
		content: GeneralVokiCreationQuestionContent;
		startEditing: () => void;
		resultsIdToName: QuestionPageResultsState;
	}
	let { content, startEditing, resultsIdToName }: Props = $props();
</script>

{#if content.$type === 'TextOnly'}
	<QuestionTextOnlyContentView {content} {resultsIdToName} />
{:else if content.$type === 'ImageOnly'}
	<QuestionImageOnlyContentView {content} {resultsIdToName} />
{:else if content.$type === 'ImageAndText'}
	<QuestionImageAndTextContentView {content} {resultsIdToName} />
{:else if content.$type === 'ColorOnly'}
	<QuestionColorOnlyContentView {content} {resultsIdToName} />
{:else if content.$type === 'ColorAndText'}
	<QuestionColorAndTextContentView {content} {resultsIdToName} />
	<!-- {:else if answer.type === 'AudioOnly'}
	<AudioOnlyAnswerView {answer} />
{:else if answer.type === 'AudioAndText'}
	<AudioAndTextAnswerView {answer} /> -->
{:else}
	<IncorrectContentTypeMessage type={content.$type} />
{/if}

<VokiCreationDefaultButton text="Edit content" onclick={startEditing} />
