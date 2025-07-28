<script lang="ts">
	import PrimaryButton from '$lib/components/PrimaryButton.svelte';
	import type { GeneralVokiAnswerTypeData, GeneralVokiAnswerType } from '$lib/ts/voki';
	import VokiCreationBasicHeader from '../../../../../c_shared/VokiCreationBasicHeader.svelte';
	import type { QuestionAnswerData } from '../../types';
	import GeneralVokiCreationNewAnswerDisplay from './c_answers_section/GeneralVokiCreationNewAnswerDisplay.svelte';
	import GeneralVokiCreationSavedAnswerDisplay from './c_answers_section/GeneralVokiCreationSavedAnswerDisplay.svelte';
	import QuestionNoAnswersDisplay from './c_answers_section/QuestionNoAnswersDisplay.svelte';

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
	function createEmptyAnswer(type: GeneralVokiAnswerType): GeneralVokiAnswerTypeData {
		switch (type) {
			case 'TextOnly':
				return { answerType: 'TextOnly', text: '' };
			case 'ImageOnly':
				return { answerType: 'ImageOnly', image: '' };
			case 'ImageAndText':
				return { answerType: 'ImageAndText', image: '', text: '' };
			case 'ColorOnly':
				return { answerType: 'ColorOnly', color: '' };
			case 'ColorAndText':
				return { answerType: 'ColorAndText', color: '', text: '' };
			case 'AudioOnly':
				return { answerType: 'AudioOnly', audio: '' };
			case 'AudioAndText':
				return { answerType: 'AudioAndText', audio: '', text: '' };
		}
	}

	function addNewAnswer() {
		unsavedAnswers.push(createEmptyAnswer(answersType));
	}
</script>

{#if answers.length + unsavedAnswers.length === 0}
	<QuestionNoAnswersDisplay {addNewAnswer} />
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
				QuestionId={questionId}
				answer={unsavedAnswer}
			/>
		{/each}
	{/if}
	{#if answers.length + unsavedAnswers.length < maxAnswersForQuestionCount}
		<PrimaryButton onclick={addNewAnswer} class="add-new-answer">Add new</PrimaryButton>
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
		width: calc(100% - 2rem);
		background-color: var(--muted);
		align-self: center;
		height: 0.125rem;
		position: relative;
	}
	.new-answer-sep label {
		position: absolute;
		top: 50%;
		left: 50%;
		transform: translate(-50%, -50%);
		background-color: var(--back);
		padding: 0.25rem 0.5rem;
		color: var(--muted-foreground);
		font-weight: 500;
		font-size: 1.375rem;
		border: 0.25rem solid var(--back);
	}
	:global(.add-new-answer.primary-btn) {
		align-self: center;
	}
</style>
