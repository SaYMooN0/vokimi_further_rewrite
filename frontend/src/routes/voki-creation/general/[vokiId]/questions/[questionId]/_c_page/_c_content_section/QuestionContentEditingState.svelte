<script lang="ts">
	import DefaultErrBlock from '$lib/components/errs/DefaultErrBlock.svelte';
	import type { Err } from '$lib/ts/err';
	import VokiCreationSaveAndCancelButtons from '../../../../../../_c_shared/VokiCreationSaveAndCancelButtons.svelte';
	import type { GeneralVokiCreationQuestionContent } from '../../types';
	import type { QuestionPageResultsState } from '../../general-voki-creation-specific-question-page-state.svelte';
	import AnswerRelatedResultsSelectingDialog from './_c_edtinig/AnswerRelatedResultsSelectingDialog.svelte';
	import QuestionImageAndTextContentEditing from './_c_edtinig/QuestionImageAndTextContentEditing.svelte';
	import QuestionTextOnlyContentEditing from './_c_edtinig/QuestionTextOnlyContentEditing.svelte';
	import QuestionImageOnlyContentEditing from './_c_edtinig/QuestionImageOnlyContentEditing.svelte';
	import QuestionColorOnlyContentEditing from './_c_edtinig/QuestionColorOnlyContentEditing.svelte';
	import QuestionColorAndTextContentEditing from './_c_edtinig/QuestionColorAndTextContentEditing.svelte';
	import IncorrectContentTypeMessage from './_c_shared/IncorrectContentTypeMessage.svelte';

	interface Props {
		content: GeneralVokiCreationQuestionContent;
		questionId: string;
		vokiId: string;
		cancelEditing: () => void;
		updateParentOnSave: (newContent: GeneralVokiCreationQuestionContent) => void;
		maxAnswersForQuestionCount: number;
		resultsIdToName: QuestionPageResultsState;
		maxResultsForAnswerCount: number;
		fetchResultNames: () => void;
	}
	let {
		content,
		questionId,
		vokiId,
		cancelEditing,
		updateParentOnSave,
		maxAnswersForQuestionCount,
		resultsIdToName,
		maxResultsForAnswerCount,
		fetchResultNames
	}: Props = $props();
	let savingErrs = $state<Err[]>([]);
	let answerRelatedResultsSelectingDialog = $state<AnswerRelatedResultsSelectingDialog>()!;
	async function saveChanges() {
		console.log('saving');
	}
</script>

{JSON.stringify(content)}
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
{:else if content.$type === 'ImageOnly'}
	<QuestionImageOnlyContentEditing
		bind:content
		{resultsIdToName}
		{maxResultsForAnswerCount}
		{maxAnswersForQuestionCount}
		openRelatedResultsSelectingDialog={(selectedResultIds, setSelected) => {
			answerRelatedResultsSelectingDialog.open(selectedResultIds, setSelected);
		}}
	/>
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
{:else if content.$type === 'ColorOnly'}
	<QuestionColorOnlyContentEditing
		bind:content
		{resultsIdToName}
		{maxResultsForAnswerCount}
		{maxAnswersForQuestionCount}
		openRelatedResultsSelectingDialog={(selectedResultIds, setSelected) => {
			answerRelatedResultsSelectingDialog.open(selectedResultIds, setSelected);
		}}
	/>
{:else if content.$type === 'ColorAndText'}
	<QuestionColorAndTextContentEditing
		bind:content
		{resultsIdToName}
		{maxResultsForAnswerCount}
		{maxAnswersForQuestionCount}
		openRelatedResultsSelectingDialog={(selectedResultIds, setSelected) => {
			answerRelatedResultsSelectingDialog.open(selectedResultIds, setSelected);
		}}
	/>
	<!--{:else if answer.type === 'AudioOnly'}
	<AudioOnlyAnswerView {answer} />
{:else if answer.type === 'AudioAndText'}
	<AudioAndTextAnswerView {answer} /> -->
{:else}
	<IncorrectContentTypeMessage type={content.$type} />
{/if}
<DefaultErrBlock errList={savingErrs} class="question-content-err-block" />
<VokiCreationSaveAndCancelButtons onCancel={() => cancelEditing()} onSave={() => saveChanges()} />
