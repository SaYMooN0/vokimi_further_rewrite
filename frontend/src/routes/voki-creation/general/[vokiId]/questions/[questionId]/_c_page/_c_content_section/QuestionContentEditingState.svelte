<script lang="ts">
	import DefaultErrBlock from '$lib/components/errs/DefaultErrBlock.svelte';
	import type { Err } from '$lib/ts/err';
	import VokiCreationSaveAndCancelButtons from '../../../../../../_c_shared/VokiCreationSaveAndCancelButtons.svelte';
	import type { GeneralVokiCreationQuestionContent } from '../../types';
	import AnswerRelatedResultsSelectingDialog from './_c_edtinig/AnswerRelatedResultsSelectingDialog.svelte';
	import QuestionImageAndTextContentEditing from './_c_edtinig/QuestionImageAndTextContentEditing.svelte';
	import QuestionTextOnlyContentEditing from './_c_edtinig/QuestionTextOnlyContentEditing.svelte';
	import IncorrectContentTypeMessage from './_c_shared/IncorrectContentTypeMessage.svelte';

	interface Props {
		content: GeneralVokiCreationQuestionContent;
		questionId: string;
		vokiId: string;
		cancelEditing: () => void;
		updateParent: (newContent: GeneralVokiCreationQuestionContent) => void;
		maxAnswersForQuestionCount: number;
		resultsIdToName: Record<string, string>;
		maxResultsForAnswerCount: number;
		fetchResultNames: () => void;
	}
	let {
		content = $bindable(),
		questionId,
		vokiId,
		cancelEditing,
		updateParent,
		maxAnswersForQuestionCount,
		resultsIdToName,
		maxResultsForAnswerCount,
		fetchResultNames
	}: Props = $props();
	let savingErrs = $state<Err[]>([]);
	let answerRelatedResultsSelectingDialog = $state<AnswerRelatedResultsSelectingDialog>()!;
	async function saveChanges() {
		console.log(content);
	}
</script>

<AnswerRelatedResultsSelectingDialog
	bind:this={answerRelatedResultsSelectingDialog}
	allResults={resultsIdToName}
	{fetchResultNames}
/>

{#if content.$type === 'TextOnly'}
	<QuestionTextOnlyContentEditing
		bind:content
		{resultsIdToName}
		{maxResultsForAnswerCount}
		{maxAnswersForQuestionCount}
		openRelatedResultsSelectingDialog={(selectedResultIds, setSelected) => {
			answerRelatedResultsSelectingDialog.open(selectedResultIds, setSelected);
		}}
	/>
	<!-- {:else if answer.type === 'ImageOnly'}
	<ImageOnlyAnswerView {answer} />-->
{:else if content.$type === 'ImageAndText'}
	<QuestionImageAndTextContentEditing
		bind:content
		{resultsIdToName}
		{maxResultsForAnswerCount}
		{maxAnswersForQuestionCount}
		openRelatedResultsSelectingDialog={(selectedResultIds, setSelected) => {
			answerRelatedResultsSelectingDialog.open(selectedResultIds, setSelected);
		}}
	/>
	<!--{:else if answer.type === 'ColorOnly'}
	<ColorOnlyAnswerView {answer} />
{:else if answer.type === 'ColorAndText'}
	<ColorAndTextAnswerView {answer} />
{:else if answer.type === 'AudioOnly'}
	<AudioOnlyAnswerView {answer} />
{:else if answer.type === 'AudioAndText'}
	<AudioAndTextAnswerView {answer} /> -->
{:else}
	<IncorrectContentTypeMessage type={content.$type} />
{/if}
<DefaultErrBlock errList={savingErrs} class="question-content-err-block" />
<VokiCreationSaveAndCancelButtons onCancel={() => cancelEditing()} onSave={() => saveChanges()} />
