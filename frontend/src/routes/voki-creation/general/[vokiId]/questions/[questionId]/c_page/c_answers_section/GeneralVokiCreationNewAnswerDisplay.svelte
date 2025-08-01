<script lang="ts">
	import DefaultErrBlock from '$lib/components/errs/DefaultErrBlock.svelte';
	import { ApiVokiCreationGeneral } from '$lib/ts/backend-communication/voki-creation-backend-service';
	import type { Err } from '$lib/ts/err';
	import { RequestJsonOptions } from '$lib/ts/request-json-options';
	import type { GeneralVokiAnswerTypeData } from '$lib/ts/voki';
	import type { QuestionAnswerData, ResultIdWithName } from '../../../types';
	import AnswerContentEditingState from './c_answer_display_contents/AnswerContentEditingState.svelte';
	import AnswerRelatedResultsEditingState from './c_answer_related_results/AnswerRelatedResultsEditingState.svelte';

	interface Props {
		vokiId: string;
		questionId: string;
		answerData: GeneralVokiAnswerTypeData;
		deleteAnswer: () => void;
		addNewSavedAnswer: (answer: QuestionAnswerData) => void;
		openRelatedResultsSelectingDialog: (
			selectedResult: ResultIdWithName[],
			setSelected: (selected: ResultIdWithName[]) => void
		) => void;
	}
	let {
		vokiId,
		questionId,
		answerData,
		deleteAnswer,
		addNewSavedAnswer,
		openRelatedResultsSelectingDialog
	}: Props = $props();
	let relatedResults: ResultIdWithName[] = $state([]);
	let savingErrs = $state<Err[]>([]);
	async function saveAnswer() {
		savingErrs = [];
		const response = await ApiVokiCreationGeneral.fetchJsonResponse<QuestionAnswerData>(
			`/vokis/${vokiId}/questions/${questionId}/answers/add-new`,
			RequestJsonOptions.POST({
				answersType: answerData.answerType,
				answerData: answerData,
				relateResultIds: relatedResults.map((r) => r.id)
			})
		);
		if (response.isSuccess) {
			console.log(response.data);
			addNewSavedAnswer(response.data);
			deleteAnswer();
		} else {
			savingErrs = response.errs;
		}
	}
	function openRelatedResultsSelectingDialogWithParams() {
		openRelatedResultsSelectingDialog(relatedResults, (selected: ResultIdWithName[]) => {
			relatedResults = selected;
		});
	}
</script>

<div class="unsaved-answer">
	<AnswerRelatedResultsEditingState
		results={relatedResults}
		openRelatedResultsSelectingDialog={openRelatedResultsSelectingDialogWithParams}
	/>
	<div class="answer-content-with-actions">
		<AnswerContentEditingState bind:answer={answerData} />
		{#if savingErrs.length > 0}
			<DefaultErrBlock errList={savingErrs} />
		{/if}
		<div class="buttons-container">
			<button class="save-btn" onclick={() => saveAnswer()}>Save</button>
			<button class="delete-btn" onclick={deleteAnswer}>
				<svg><use href="#common-trash-can-icon" /></svg>
			</button>
		</div>
	</div>
</div>

<style>
	.unsaved-answer {
		display: grid;
		grid-template-columns: 13rem 1fr;
		gap: 0.25rem;
		width: 100%;
		padding: 0.5rem 0.75rem;
		margin-top: 1rem;
		border: 0.125rem dashed var(--secondary-foreground);
		border-radius: 0.75rem;
	}

	.answer-content-with-actions {
		width: 100%;
		display: flex;
		flex-direction: column;
		gap: 0.75rem;
	}

	.buttons-container {
		display: flex;
		flex-direction: row;
		justify-content: flex-end;
		gap: 0.5rem;
		width: 100%;
	}

	.buttons-container > * {
		display: flex;
		justify-content: center;
		align-items: center;
		height: 2rem;
		border: none;
		border-radius: 0.25rem;
		box-shadow: var(--shadow);
		outline: none;
		cursor: pointer;
	}

	.save-btn {
		padding: 0 1.25rem;
		background-color: var(--primary);
		color: var(--primary-foreground);
		font-size: 1.25rem;
		font-weight: 480;
		letter-spacing: 1.5px;
	}

	.save-btn:hover {
		background-color: var(--primary-hov);
	}

	.delete-btn {
		width: 2rem;
		background-color: var(--muted);
		color: var(--muted-foreground);
		stroke-width: 2.1;
	}

	.delete-btn:hover {
		background-color: var(--err-foreground);
		color: var(--primary-foreground);
		stroke-width: 1.8;
	}

	.delete-btn svg {
		width: 1.5rem;
		height: 1.5rem;
	}
</style>
