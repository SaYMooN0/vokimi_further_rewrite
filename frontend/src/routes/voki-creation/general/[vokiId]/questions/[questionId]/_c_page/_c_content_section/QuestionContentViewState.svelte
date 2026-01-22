<script lang="ts">
	import VokiCreationDefaultButton from '../../../../../../_c_shared/VokiCreationDefaultButton.svelte';
	import type { GeneralVokiCreationQuestionContent } from '../../types';
	import IncorrectContentTypeMessage from './_c_shared/IncorrectContentTypeMessage.svelte';
	import QuestionTextOnlyContentView from './_c_view/QuestionTextOnlyContentView.svelte';

	interface Props {
		savedTypeSpecificContent: GeneralVokiCreationQuestionContent;
		startEditing: () => void;
		resultsIdToName: Record<string, string>;
	}
	let { savedTypeSpecificContent, startEditing, resultsIdToName }: Props = $props();
</script>

{#if savedTypeSpecificContent.$type === 'TextOnly'}
	<QuestionTextOnlyContentView content={savedTypeSpecificContent} {resultsIdToName} />
	<!-- {:else if answer.type === 'ImageOnly'}
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
	<AudioAndTextAnswerView {answer} /> -->
{:else}
	<IncorrectContentTypeMessage type={savedTypeSpecificContent.$type} />
{/if}

<VokiCreationDefaultButton text="Edit content" onclick={startEditing} />
