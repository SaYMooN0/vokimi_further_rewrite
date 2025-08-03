<script lang="ts">
	import { ApiVokiCreationGeneral } from '$lib/ts/backend-communication/voki-creation-backend-service';
	import { RequestJsonOptions } from '$lib/ts/request-json-options';
	import type { GeneralVokiAnswerTypeData } from '$lib/ts/voki';
	import { toast } from 'svelte-sonner';
	import type { QuestionAnswerData } from '../../../types';
	import SavedAnswerDisplayEditing from './c_saved_answer/SavedAnswerDisplayEditing.svelte';

	interface Props {
		vokiId: string;
		questionId: string;
		answer: QuestionAnswerData;
		refetchOnDelete: () => void;
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
		refetchOnDelete,
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
			bind:answer
			{vokiId}
			{questionId}
			{openRelatedResultsSelectingDialog}
			{updateParentOnSave}
			cancelEditing={() => (currentState = { isEditing: false })}
		/>
	{:else}
		<!-- <SavedAnswerDisplayView {answer} {vokiId} {questionId} /> -->
	{/if}
</div>

<style>
	.saved-answer {
		display: flex;
		flex-direction: column;
		place-items: center center;
		gap: 0.75rem;
		width: 100%;
		padding: 0.5rem 0.75rem;
		margin-top: 1rem;
		border: 0.125rem solid var(--secondary-foreground);
		border-radius: 0.75rem;
	}
	.saved-answer.editing {
		border: 0.125rem dashed var(--primary);
	}
</style>
