<script lang="ts">
	import { ApiVokiCreationGeneral } from '$lib/ts/backend-communication/voki-creation-backend-service';
	import type { Err } from '$lib/ts/err';
	import { RequestJsonOptions } from '$lib/ts/request-json-options';
	import type { QuestionAnswerData } from '../../../../types';
	import AnswerDisplayContentWrapper from '../AnswerDisplayContentWrapper.svelte';
	import AnswerContentEditingState from '../c_answer_display_contents/c_answer_content_editing_state/AnswerContentEditingState.svelte';
	import AnswerRelatedResultsEditingState from '../c_answer_display_contents/c_answer_content_editing_state/AnswerRelatedResultsEditingState.svelte';

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
		answer = $bindable(),
		openRelatedResultsSelectingDialog,
		vokiId,
		questionId,
		updateParentOnSave,
		cancelEditing
	}: Props = $props();

	let savingErrs = $state<Err[]>([]);
	async function saveAnswer() {
		savingErrs = [];
		const { relatedResultIds, ...answerWithoutRelated } = answer;
		const response = await ApiVokiCreationGeneral.fetchJsonResponse<QuestionAnswerData>(
			`/vokis/${vokiId}/questions/${questionId}/answers/${answer.id}/update`,
			RequestJsonOptions.POST({
				relatedResultIds,
				...answerWithoutRelated
			})
		);
		if (response.isSuccess) {
			updateParentOnSave(response.data);
		} else {
			savingErrs = response.errs;
		}
	}

	function openRelatedResultsSelectingDialogWithParams() {
		openRelatedResultsSelectingDialog(answer.relatedResultIds, (selected: string[]) => {
			answer.relatedResultIds = selected;
		});
	}
</script>

<AnswerRelatedResultsEditingState
	relatedResultIds={answer.relatedResultIds}
	openRelatedResultsSelectingDialog={openRelatedResultsSelectingDialogWithParams}
/>
<AnswerDisplayContentWrapper
	errs={savingErrs}
	mainBtnText="Save"
	mainBtnOnClick={() => saveAnswer()}
	secondaryBtnIconId="#common-cross-icon"
	secondaryBtnOnClick={() => cancelEditing()}
>
	<AnswerContentEditingState bind:answer={answer.typeData} />
</AnswerDisplayContentWrapper>
