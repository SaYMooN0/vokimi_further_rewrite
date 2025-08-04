<script lang="ts">
	import PrimaryButton from '$lib/components/PrimaryButton.svelte';
	import type { GeneralVokiAnswerTypeData, GeneralVokiAnswerType } from '$lib/ts/voki';
	import ListEmptyMessage from '../../../../../c_shared/ListEmptyMessage.svelte';
	import VokiCreationBasicHeader from '../../../../../c_shared/VokiCreationBasicHeader.svelte';
	import { type QuestionAnswerData, createEmptyGeneralVokiAnswerTypeData } from '../types';
	import AnswerRelatedResultsSelectingDialog from './c_answers_section/AnswerRelatedResultsSelectingDialog.svelte';
	import GeneralVokiCreationNewAnswerDisplay from './c_answers_section/c_answer_displays/GeneralVokiCreationNewAnswerDisplay.svelte';
	import GeneralVokiCreationSavedAnswerDisplay from './c_answers_section/c_answer_displays/GeneralVokiCreationSavedAnswerDisplay.svelte';

	let {
		questionId,
		vokiId,
		answers,
		answersType
	}: {
		questionId: string;
		vokiId: string;
		answers: QuestionAnswerData[];
		answersType: GeneralVokiAnswerType;
	} = $props<{
		questionId: string;
		vokiId: string;
		answers: QuestionAnswerData[];
		answersType: GeneralVokiAnswerType;
	}>();
	const maxAnswersForQuestionCount = 60;
	let unsavedAnswers = $state<GeneralVokiAnswerTypeData[]>([]);
	let resultsSelectingDialog = $state<AnswerRelatedResultsSelectingDialog>()!;

	function addNewUnsavedAnswer() {
		unsavedAnswers.push(createEmptyGeneralVokiAnswerTypeData(answersType));
	}
	function addNewSavedAnswer(answer: QuestionAnswerData) {
		answers.push(answer);
		answers.sort((a, b) => a.order - b.order);
	}
	function updateAnswerOnSave(newAnswer: QuestionAnswerData) {
		answers = answers.map((a) => (a.id === newAnswer.id ? newAnswer : a));
		answers.sort((a, b) => a.order - b.order);
	}
</script>

<AnswerRelatedResultsSelectingDialog bind:this={resultsSelectingDialog} {vokiId} />
{#if answers.length + unsavedAnswers.length === 0}
	<ListEmptyMessage
		onBtnClick={addNewUnsavedAnswer}
		messageText="This question has no answers yet"
		btnText="Add first answer"
	/>
{:else}
	<VokiCreationBasicHeader
		header="Question answers ({answers.length}{unsavedAnswers.length != 0
			? ` + ${unsavedAnswers.length}*`
			: ''})"
	/>
	{#each answers as answer}
		<GeneralVokiCreationSavedAnswerDisplay
			{answer}
			{vokiId}
			{questionId}
			openRelatedResultsSelectingDialog={(selected, setSelected) =>
				resultsSelectingDialog.open(selected, setSelected)}
			updateParentOnSave={updateAnswerOnSave}
			refetchOnDelete={() => {}}
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
		<PrimaryButton onclick={addNewUnsavedAnswer} class="add-new-answer">Add new</PrimaryButton>
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
</style>
