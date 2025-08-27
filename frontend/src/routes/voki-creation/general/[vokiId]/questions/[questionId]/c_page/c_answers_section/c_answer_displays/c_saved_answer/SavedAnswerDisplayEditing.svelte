<script lang="ts">
	import { ApiVokiCreationGeneral } from '$lib/ts/backend-communication/voki-creation-backend-service';
	import type { Err } from '$lib/ts/err';
	import { RequestJsonOptions } from '$lib/ts/request-json-options';
	import type { GeneralVokiCreationAnswerData, QuestionAnswerData } from '../../../../types';
	import AnswerDisplayContentWrapper from '../AnswerDisplayContentWrapper.svelte';
	import AnswerContentEditingState from '../c_answer_display_contents/c_editing/AnswerContentEditingState.svelte';
	import AnswerRelatedResultsEditingState from '../c_answer_display_contents/c_editing/AnswerRelatedResultsEditingState.svelte';
	import AnswerDisplayVerticalSep from '../c_answer_display_contents/c_shared/AnswerDisplayVerticalSep.svelte';

	interface Props {
		answer: QuestionAnswerData;
		openRelatedResultsSelectingDialog: (
			selectedResultIds: string[],
			setSelected: (selected: string[]) => void
		) => void;
		vokiId: string;
		questionId: string;
		updateParentOnSave: (answer: QuestionAnswerData) => void;
		cancelEditing: () => void;
	}
	let {
		answer,
		openRelatedResultsSelectingDialog,
		vokiId,
		questionId,
		updateParentOnSave,
		cancelEditing
	}: Props = $props();

	let savingErrs = $state<Err[]>([]);
	let answerData = $state<GeneralVokiCreationAnswerData>(answer.typeData);
	let answerResults = $state<string[]>(answer.relatedResultIds);

	async function saveAnswer() {
		savingErrs = [];
		const response = await ApiVokiCreationGeneral.fetchJsonResponse<QuestionAnswerData>(
			`/vokis/${vokiId}/questions/${questionId}/answers/${answer.id}/update`,
			RequestJsonOptions.PUT({
				relateResultIds: answerResults,
				answerData: answerData
			})
		);
		if (response.isSuccess) {
			updateParentOnSave(response.data);
			cancelEditing();
		} else {
			savingErrs = response.errs;
		}
	}

	function openRelatedResultsSelectingDialogWithParams() {
		openRelatedResultsSelectingDialog(answerResults, (selected: string[]) => {
			answerResults = selected;
		});
	}
	function removeResult(resultId: string) {
		answerResults = answerResults.filter((id) => id != resultId);
	}
</script>

<AnswerRelatedResultsEditingState
	relatedResultIds={answerResults}
	openRelatedResultsSelectingDialog={openRelatedResultsSelectingDialogWithParams}
	{removeResult}
/>
<AnswerDisplayVerticalSep />
<AnswerDisplayContentWrapper
	errs={savingErrs}
	mainBtnText="Save"
	mainBtnOnClick={() => saveAnswer()}
	secondaryBtnIconId="#common-cross-icon"
	secondaryBtnOnClick={() => cancelEditing()}
>
	<AnswerContentEditingState bind:answer={answerData} />
</AnswerDisplayContentWrapper>
