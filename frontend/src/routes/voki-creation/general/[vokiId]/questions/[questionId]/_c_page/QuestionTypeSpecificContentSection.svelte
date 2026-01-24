<script lang="ts">
	import VokiCreationBasicHeader from '../../../../../_c_shared/VokiCreationBasicHeader.svelte';
	import type { GeneralVokiCreationQuestionContent } from '../types';
	import QuestionContentEditingState from './_c_content_section/QuestionContentEditingState.svelte';
	import type { QuestionPageResultsState } from '../general-voki-creation-specific-question-page-state.svelte';
	import QuestionContentViewState from './_c_content_section/QuestionContentViewState.svelte';

	interface Props {
		savedTypeSpecificContent: GeneralVokiCreationQuestionContent;
		questionId: string;
		vokiId: string;
		updateSavedTypeSpecificContent: (
			newTypeSpecificContent: GeneralVokiCreationQuestionContent
		) => void;
		isEditing: boolean;
		resultsIdToName: QuestionPageResultsState;
		maxResultsForAnswerCount: number;
		maxAnswersForQuestionCount: number;
		fetchResultNames: () => void;
	}
	let {
		savedTypeSpecificContent,
		questionId,
		vokiId,
		updateSavedTypeSpecificContent,
		isEditing = $bindable(),
		resultsIdToName,
		maxResultsForAnswerCount,
		maxAnswersForQuestionCount,
		fetchResultNames
	}: Props = $props();
	function startEditing() {
		// editingContent = savedTypeSpecificContent;
		isEditing = true;
	}
	function cancelEditing() {
		isEditing = false;
	}
	function updateParentOnSave(newContent: GeneralVokiCreationQuestionContent) {
		updateSavedTypeSpecificContent(newContent);
		isEditing = false;
	}
</script>

<VokiCreationBasicHeader header="{isEditing ? '*' : ''}Question type specific content" />
{JSON.stringify(savedTypeSpecificContent)}
<br />
{#if isEditing}
	<QuestionContentEditingState
		content={savedTypeSpecificContent}
		{questionId}
		{vokiId}
		{cancelEditing}
		{updateParentOnSave}
		{maxAnswersForQuestionCount}
		{resultsIdToName}
		{maxResultsForAnswerCount}
		{fetchResultNames}
	/>
{:else}
	<QuestionContentViewState {savedTypeSpecificContent} {startEditing} {resultsIdToName} />
{/if}
