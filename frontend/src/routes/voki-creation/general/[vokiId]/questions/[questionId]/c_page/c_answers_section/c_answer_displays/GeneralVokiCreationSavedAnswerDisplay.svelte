<script lang="ts">
	import type { GeneralVokiAnswerTypeData } from '$lib/ts/voki';
	import type { QuestionAnswerData } from '../../../types';
	import SavedAnswerDisplayEditing from './c_saved_answer/SavedAnswerDisplayEditing.svelte';
	import SavedAnswerDisplayView from './c_saved_answer/SavedAnswerDisplayView.svelte';

	interface Props {
		vokiId: string;
		questionId: string;
		answer: QuestionAnswerData;
		updateParentOnDelete: (answerId : string) => void;
		updateParentOnSave: (answer: QuestionAnswerData) => void;
		openRelatedResultsSelectingDialog: (
			selectedResultIds: string[],
			setSelected: (selected: string[]) => void
		) => void;
	}
	let {
		vokiId,
		questionId,
		answer,
		updateParentOnDelete,
		updateParentOnSave,
		openRelatedResultsSelectingDialog
	}: Props = $props();

	type isEditingState =
		| { isEditing: true; answer: GeneralVokiAnswerTypeData }
		| { isEditing: false };
	let currentState = $state<isEditingState>({ isEditing: false });
</script>

<div class="saved-answer" class:editing={currentState.isEditing}>
	{#if currentState.isEditing}
		<SavedAnswerDisplayEditing
			{answer}
			{vokiId}
			{questionId}
			{openRelatedResultsSelectingDialog}
			{updateParentOnSave}
			cancelEditing={() => (currentState = { isEditing: false })}
		/>
	{:else}
		<SavedAnswerDisplayView
			{answer}
			{vokiId}
			{questionId}
			startEditing={() => (currentState = { isEditing: true, answer: answer.typeData })}
			{updateParentOnDelete}
		/>
	{/if}
</div>

<style>
	.saved-answer {
		--results-width: 13rem;

		display: grid;
		grid-template-columns: var(--results-width) auto 1fr;
		gap: 0.25rem;
		width: 100%;
		padding: 0.5rem 0.75rem;
		margin-top: 1rem;
		border-radius: 0.75rem;
		box-shadow: rgb(0 0 0 / 5%) 0 0 0 1px;
	}

	.saved-answer.editing {
		border: 0.125rem dashed var(--primary);
		box-shadow: none;
	}
</style>
