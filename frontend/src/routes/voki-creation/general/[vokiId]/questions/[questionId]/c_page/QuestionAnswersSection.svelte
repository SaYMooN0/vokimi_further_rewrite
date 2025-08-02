<script lang="ts">
	import PrimaryButton from '$lib/components/PrimaryButton.svelte';
	import type { GeneralVokiAnswerTypeData, GeneralVokiAnswerType } from '$lib/ts/voki';
	import ListEmptyMessage from '../../../../../c_shared/ListEmptyMessage.svelte';
	import VokiCreationBasicHeader from '../../../../../c_shared/VokiCreationBasicHeader.svelte';
	import type { QuestionAnswerData } from '../../types';
	import AnswerRelatedResultsSelectingDialog from './c_answers_section/AnswerRelatedResultsSelectingDialog.svelte';
	import GeneralVokiCreationNewAnswerDisplay from './c_answers_section/GeneralVokiCreationNewAnswerDisplay.svelte';
	import GeneralVokiCreationSavedAnswerDisplay from './c_answers_section/GeneralVokiCreationSavedAnswerDisplay.svelte';

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
	function createEmptyAnswer(type: GeneralVokiAnswerType): GeneralVokiAnswerTypeData {
		switch (type) {
			case 'TextOnly':
				return { type: 'TextOnly', relatedResultIds: [], text: '' };
			case 'ImageOnly':
				return { type: 'ImageOnly', relatedResultIds: [], image: '' };
			case 'ImageAndText':
				return { type: 'ImageAndText', relatedResultIds: [], image: '', text: '' };
			case 'ColorOnly':
				return { type: 'ColorOnly', relatedResultIds: [], color: '' };
			case 'ColorAndText':
				return { type: 'ColorAndText', relatedResultIds: [], color: '', text: '' };
			case 'AudioOnly':
				return { type: 'AudioOnly', relatedResultIds: [], audio: '' };
			case 'AudioAndText':
				return { type: 'AudioAndText', relatedResultIds: [], audio: '', text: '' };
		}
	}

	function addNewUnsavedAnswer() {
		unsavedAnswers.push(createEmptyAnswer(answersType));
	}
	function addNewSavedAnswer(answer: QuestionAnswerData) {
		answers.push(answer);
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
		<GeneralVokiCreationSavedAnswerDisplay {vokiId} QuestionId={questionId} {answer} />
	{/each}
	{#if unsavedAnswers.length != 0}
		<div class="new-answer-sep">
			<label>New answers ({unsavedAnswers.length}*)</label>
		</div>
		{#each unsavedAnswers as unsavedAnswer}
			<GeneralVokiCreationNewAnswerDisplay
				{vokiId}
				{questionId}
				answer={unsavedAnswer}
				deleteAnswer={() => {
					unsavedAnswers = unsavedAnswers.filter((a) => a != unsavedAnswer);
				}}
				{addNewSavedAnswer}
				openRelatedResultsSelectingDialog={(selectedIds, setSelected) =>
					resultsSelectingDialog.open(selectedIds, setSelected)}
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
		background-color: var(--muted);
		align-self: center;
		margin: 2rem 0;
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
