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
	import { ApiVokiCreationGeneral } from '$lib/ts/backend-communication/voki-creation-backend-service';
	import { RJO } from '$lib/ts/backend-communication/backend-services';

	interface Props {
		savedContent: GeneralVokiCreationQuestionContent;
		questionId: string;
		vokiId: string;
		cancelEditing: () => void;
		updateParentOnSave: (newContent: GeneralVokiCreationQuestionContent) => void;
		maxAnswersForQuestionCount: number;
		resultsIdToNameState: QuestionPageResultsState;
		maxResultsForAnswerCount: number;
		fetchResultNames: () => void;
	}
	let {
		savedContent,
		questionId,
		vokiId,
		cancelEditing,
		updateParentOnSave,
		maxAnswersForQuestionCount,
		resultsIdToNameState,
		maxResultsForAnswerCount,
		fetchResultNames
	}: Props = $props();
	let savingErrs = $state<Err[]>([]);
	let answerRelatedResultsSelectingDialog = $state<AnswerRelatedResultsSelectingDialog>()!;
	async function saveChanges() {
		const response =
			await ApiVokiCreationGeneral.fetchJsonResponse<GeneralVokiCreationQuestionContent>(
				`vokis/${vokiId}/questions/${questionId}/update-content`,
				RJO.PATCH(content)
			);
		if (response.isSuccess) {
			updateParentOnSave(response.data);
		} else {
			savingErrs = response.errs;
		}
	}
	let content = $state(savedContent);
</script>

<AnswerRelatedResultsSelectingDialog
	bind:this={answerRelatedResultsSelectingDialog}
	allResults={resultsIdToNameState}
	{fetchResultNames}
/>

{#if content.$type === 'TextOnly'}
	<QuestionTextOnlyContentEditing
		bind:content
		{resultsIdToNameState}
		{maxResultsForAnswerCount}
		{maxAnswersForQuestionCount}
		openRelatedResultsSelectingDialog={(selectedResultIds, setSelected) => {
			answerRelatedResultsSelectingDialog.open(selectedResultIds, setSelected);
		}}
	/>
{:else if content.$type === 'ImageOnly'}
	<QuestionImageOnlyContentEditing
		bind:content
		{resultsIdToNameState}
		{maxResultsForAnswerCount}
		{maxAnswersForQuestionCount}
		openRelatedResultsSelectingDialog={(selectedResultIds, setSelected) => {
			answerRelatedResultsSelectingDialog.open(selectedResultIds, setSelected);
		}}
	/>
{:else if content.$type === 'ImageAndText'}
	<QuestionImageAndTextContentEditing
		bind:content
		{resultsIdToNameState}
		{maxResultsForAnswerCount}
		{maxAnswersForQuestionCount}
		openRelatedResultsSelectingDialog={(selectedResultIds, setSelected) => {
			answerRelatedResultsSelectingDialog.open(selectedResultIds, setSelected);
		}}
	/>
{:else if content.$type === 'ColorOnly'}
	<QuestionColorOnlyContentEditing
		bind:content
		{resultsIdToNameState}
		{maxResultsForAnswerCount}
		{maxAnswersForQuestionCount}
		openRelatedResultsSelectingDialog={(selectedResultIds, setSelected) => {
			answerRelatedResultsSelectingDialog.open(selectedResultIds, setSelected);
		}}
	/>
{:else if content.$type === 'ColorAndText'}
	<QuestionColorAndTextContentEditing
		bind:content
		{resultsIdToNameState}
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
