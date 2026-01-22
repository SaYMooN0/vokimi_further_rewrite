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
	}
	let {
		savedTypeSpecificContent,
		questionId,
		vokiId,
		updateSavedTypeSpecificContent,
		isEditing = $bindable(),
		resultsIdToName
	}: Props = $props();
</script>

<VokiCreationBasicHeader header="Question content" />
{#if isEditing}
	<QuestionContentEditingState
		savedContent={savedTypeSpecificContent}
		{questionId}
		{vokiId}
		cancelEditing={() => (isEditing = false)}
		updateParent={(newContent) => {
			updateSavedTypeSpecificContent(newContent);
			isEditing = false;
		}}
	/>
{:else}
	<QuestionContentViewState
		{savedTypeSpecificContent}
		startEditing={() => (isEditing = true)}
		{resultsIdToName}
	/>
{/if}
<!--
<AnswerRelatedResultsSelectingDialog bind:this={resultsSelectingDialog} {vokiId} />
{#if answers.length + unsavedAnswers.length === 0}
	<ListEmptyMessage
		onBtnClick={addNewUnsavedAnswer}
		messageText="This question has no answers yet"
		btnText="Add first answer"
	/>
{:else}
	
	{#each answers as answer}
		<GeneralVokiCreationSavedAnswerDisplay
			{answer}
			{vokiId}
			{questionId}
			openRelatedResultsSelectingDialog={(selected, setSelected) =>
				resultsSelectingDialog.open(selected, setSelected)}
			updateParentOnSave={updateAnswerOnSave}
			updateParentOnDelete={(id) => updateOnAnswerDelete(id)}
		/>
	{/each}
	{#if unsavedAnswers.length != 0}
		<div class="new-answer-sep">
			<label>New answers ({unsavedAnswers.length}*)</label>
		</div>
		{#each unsavedAnswers as unsavedAnswer, i}
			<GeneralVokiCreationNewAnswerDisplay
				bind:answer={unsavedAnswers[i]}
				{vokiId}
				{questionId}
				openRelatedResultsSelectingDialog={(selected, setSelected) =>
					resultsSelectingDialog.open(selected, setSelected)}
				{addNewSavedAnswer}
				deleteAnswer={() => {
					unsavedAnswers = unsavedAnswers.filter((a) => a != unsavedAnswer);
				}}
			/>
		{/each}
	{/if}
	{#if answers.length + unsavedAnswers.length < maxAnswersForQuestionCount}
		<PrimaryButton onclick={addNewUnsavedAnswer} class="add-new-answer"
			>Add new answer</PrimaryButton
		>
	{:else}
		<div class="limit-reached-message">
			<h1>
				The limit of {maxAnswersForQuestionCount} questions has been reached
			</h1>
		</div>
	{/if}
{/if}

<style>
	.new-answer-sep {
		position: relative;
		width: calc(100% - 2rem);
		height: 0.125rem;
		margin: 2rem 0;
		background-color: var(--muted);
		align-self: center;
	}

	.new-answer-sep label {
		position: absolute;
		top: 50%;
		left: 50%;
		padding: 0.25rem 0.5rem;
		border: 0.25rem solid var(--back);
		background-color: var(--back);
		color: var(--muted-foreground);
		font-size: 1.375rem;
		font-weight: 500;
		transform: translate(-50%, -50%);
	}

	:global(.add-new-answer.primary-btn) {
		padding: 0.25rem 2rem;
		margin: 1.5rem;
		align-self: center;
	}
</style> -->
