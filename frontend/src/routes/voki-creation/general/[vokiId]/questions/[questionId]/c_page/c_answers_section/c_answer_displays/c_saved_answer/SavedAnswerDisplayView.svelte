<script lang="ts">
	import { ApiVokiCreationGeneral } from '$lib/ts/backend-communication/voki-creation-backend-service';
	import { toast } from 'svelte-sonner';
	import type { QuestionAnswerData } from '../../../../types';
	import AnswerDisplayContentWrapper from '../AnswerDisplayContentWrapper.svelte';
	import AnswerContentViewState from '../c_answer_display_contents/c_view/AnswerContentViewState.svelte';
	import AnswerRelatedResultsViewState from '../c_answer_display_contents/c_view/AnswerRelatedResultsViewState.svelte';
	import AnswerDisplayVerticalSep from '../c_answer_display_contents/c_shared/AnswerDisplayVerticalSep.svelte';
	import { getConfirmActionDialogOpenFunction } from '../../../../../../../../../c_layout/ts_layout_contexts/confirm-action-dialog-context';
	import { RJO } from '$lib/ts/backend-communication/backend-services';

	interface Props {
		answer: QuestionAnswerData;
		vokiId: string;
		questionId: string;
		startEditing: () => void;
		updateParentOnDelete: (answerId: string) => void;
	}
	let {
		answer = $bindable(),
		vokiId,
		questionId,
		startEditing,
		updateParentOnDelete
	}: Props = $props();

	const { open: openConfirmationDialog, close: closeConfirmationDialog } =
		getConfirmActionDialogOpenFunction();

	function openAnswerDeleteConfirmationDialog() {
		const deleteAnswer = async () => {
			const response = await ApiVokiCreationGeneral.fetchVoidResponse(
				`/vokis/${vokiId}/questions/${questionId}/answers/${answer.id}/delete`,
				RJO.DELETE({})
			);
			if (response.isSuccess) {
				updateParentOnDelete(answer.id);
				closeConfirmationDialog();
				return [];
			} else {
				return response.errs;
			}
		};
		openConfirmationDialog({
			mainContent: {
				mainText: 'Are you sure you want to delete this answer?',
				subheading: undefined
			},
			dialogButtons: {
				confirmBtnText: 'Delete',
				confirmBtnOnclick: () => deleteAnswer(),
				cancelBtnText: 'Cancel',
				cancelBtnOnclick: closeConfirmationDialog
			}
		});
	}
</script>

<AnswerRelatedResultsViewState relatedResultIds={answer.relatedResultIds} />
<AnswerDisplayVerticalSep />
<AnswerDisplayContentWrapper
	errs={[]}
	mainBtnText="Edit"
	mainBtnOnClick={() => startEditing()}
	secondaryBtnIconId="#common-trash-can-icon"
	secondaryBtnOnClick={() => openAnswerDeleteConfirmationDialog()}
>
	<AnswerContentViewState answer={answer.typeData} />
</AnswerDisplayContentWrapper>
