<script lang="ts">
	import { RJO } from '$lib/ts/backend-communication/backend-services';
	import { ApiVokiCreationGeneral } from '$lib/ts/backend-communication/voki-creation-backend-service';
	import type { Err } from '$lib/ts/err';
	import type { GeneralVokiCreationAnswerData, QuestionAnswerData } from '../../../types';
	import AnswerDisplayContentWrapper from './AnswerDisplayContentWrapper.svelte';
	import AnswerContentEditingState from './c_answer_display_contents/c_editing/AnswerContentEditingState.svelte';
	import AnswerRelatedResultsEditingState from './c_answer_display_contents/c_editing/AnswerRelatedResultsEditingState.svelte';

	interface Props {
		vokiId: string;
		questionId: string;
		answer: GeneralVokiCreationAnswerData;
		deleteAnswer: () => void;
		addNewSavedAnswer: (answer: QuestionAnswerData) => void;
		openRelatedResultsSelectingDialog: (
			selectedResultIds: string[],
			setSelected: (selected: string[]) => void
		) => void;
	}
	let {
		vokiId,
		questionId,
		answer = $bindable(),
		deleteAnswer,
		addNewSavedAnswer,
		openRelatedResultsSelectingDialog
	}: Props = $props();
	let savingErrs = $state<Err[]>([]);
	async function saveAnswer() {
		savingErrs = [];
		const { relatedResultIds, ...answerWithoutRelated } = answer;

		const response = await ApiVokiCreationGeneral.fetchJsonResponse<QuestionAnswerData>(
			`/vokis/${vokiId}/questions/${questionId}/answers/add-new`,
			RJO.POST({
				relateResultIds: relatedResultIds,
				answerData: answerWithoutRelated
			})
		);
		if (response.isSuccess) {
			addNewSavedAnswer(response.data);
			deleteAnswer();
		} else {
			savingErrs = response.errs;
		}
	}
	function openRelatedResultsSelectingDialogWithParams() {
		openRelatedResultsSelectingDialog(answer.relatedResultIds, (selected: string[]) => {
			answer.relatedResultIds = selected;
		});
	}
	function removeResult(resultId: string) {
		answer.relatedResultIds = answer.relatedResultIds.filter((id) => id != resultId);
	}
</script>

<div class="unsaved-answer">
	<AnswerRelatedResultsEditingState
		relatedResultIds={answer.relatedResultIds}
		openRelatedResultsSelectingDialog={openRelatedResultsSelectingDialogWithParams}
		{removeResult}
	/>
	<AnswerDisplayContentWrapper
		errs={savingErrs}
		mainBtnText="Save"
		mainBtnOnClick={() => saveAnswer()}
		secondaryBtnIconId="#common-trash-can-icon"
		secondaryBtnOnClick={() => deleteAnswer()}
	>
		<AnswerContentEditingState bind:answer />
	</AnswerDisplayContentWrapper>
</div>

<style>
	.unsaved-answer {
		--results-width: 13rem;

		display: grid;
		grid-template-columns: var(--results-width) 1fr;
		gap: 0.25rem;
		width: 100%;
		padding: 0.5rem 0.75rem;
		margin-top: 1rem;
		border: 0.125rem dashed var(--secondary-foreground);
		border-radius: 0.75rem;
	}
</style>
