<script lang="ts">
	import { ApiVokiCreationGeneral } from '$lib/ts/backend-communication/voki-creation-backend-service';
	import { RequestJsonOptions } from '$lib/ts/request-json-options';
	import { toast } from 'svelte-sonner';
	import type { QuestionAnswerData } from '../../../../types';
	import AnswerDisplayContentWrapper from '../AnswerDisplayContentWrapper.svelte';
	import AnswerContentViewState from '../c_answer_display_contents/c_answer_content_view_state/AnswerContentViewState.svelte';
	import AnswerRelatedResultsViewState from '../c_answer_display_contents/c_answer_content_view_state/AnswerRelatedResultsViewState.svelte';

	interface Props {
		answer: QuestionAnswerData;
		vokiId: string;
		questionId: string;
		startEditing: () => void;
		refetchOnDelete: () => void;
	}
	let { answer = $bindable(), vokiId, questionId, startEditing, refetchOnDelete }: Props = $props();

	async function deleteAnswer() {
		const response = await ApiVokiCreationGeneral.fetchJsonResponse<void>(
			`/vokis/${vokiId}/questions/${questionId}/answers/${answer.id}/delete`,
			RequestJsonOptions.DELETE({})
		);
		if (response.isSuccess) {
			refetchOnDelete();
		} else {
			toast.error('Failed to delete answer');
		}
	}
</script>

<AnswerRelatedResultsViewState relatedResultIds={answer.relatedResultIds} />
<AnswerDisplayContentWrapper
	errs={[]}
	mainBtnText="Save"
	mainBtnOnClick={() => startEditing()}
	secondaryBtnIconId="#common-trash-can-icon"
	secondaryBtnOnClick={() => deleteAnswer()}
>
	<AnswerContentViewState answer={answer.typeData} />
</AnswerDisplayContentWrapper>
