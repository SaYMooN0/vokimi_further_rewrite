<script lang="ts">
	import VokiCreationBasicHeader from '../../../../../_c_shared/VokiCreationBasicHeader.svelte';
	import type { GeneralVokiCreationQuestionContent } from '../types';
	import QuestionContentEditingState from './_c_content_section/QuestionContentEditingState.svelte';
	import QuestionContentViewState from './_c_content_section/QuestionContentViewState.svelte';

	interface Props {
		savedTypeSpecificContent: GeneralVokiCreationQuestionContent;
		questionId: string;
		vokiId: string;
		updateSavedTypeSpecificContent: (
			newTypeSpecificContent: GeneralVokiCreationQuestionContent
		) => void;
		isEditing: boolean;
		resultsIdToName: Record<string, string>;
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
		editingContent = savedTypeSpecificContent;
		isEditing = true;
	}
	let editingContent = $state<GeneralVokiCreationQuestionContent>(savedTypeSpecificContent);
</script>

<VokiCreationBasicHeader header="{isEditing ? '*' : ''}Question type specific content" />
{#if isEditing}
	<QuestionContentEditingState
		bind:content={editingContent}
		{questionId}
		{vokiId}
		cancelEditing={() => (isEditing = false)}
		updateParent={(newContent) => {
			updateSavedTypeSpecificContent(newContent);
			isEditing = false;
		}}
		{maxAnswersForQuestionCount}
		{resultsIdToName}
		{maxResultsForAnswerCount}
		{fetchResultNames}
	/>
{:else}
	<QuestionContentViewState {savedTypeSpecificContent} {startEditing} {resultsIdToName} />
{/if}
