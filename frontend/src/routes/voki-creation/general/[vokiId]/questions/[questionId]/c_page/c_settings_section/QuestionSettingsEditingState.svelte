<script lang="ts">
	import DefaultErrBlock from '$lib/components/errs/DefaultErrBlock.svelte';
	import TwoStateSwitchInput from '$lib/components/inputs/TwoStateSwitchInput.svelte';
	import { RJO } from '$lib/ts/backend-communication/backend-services';
	import { ApiVokiCreationGeneral } from '$lib/ts/backend-communication/voki-creation-backend-service';
	import type { Err } from '$lib/ts/err';
	import VokiCreationFieldName from '../../../../../../c_shared/VokiCreationFieldName.svelte';
	import VokiCreationSaveAndCancelButtons from '../../../../../../c_shared/VokiCreationSaveAndCancelButtons.svelte';

	interface Props {
		shuffleAnswers: boolean;
		minAnswers: number;
		maxAnswers: number;
		questionId: string;
		vokiId: string;
		cancelEditing: () => void;
		updateParent: (newSettings: {
			shuffleAnswers: boolean;
			minAnswers: number;
			maxAnswers: number;
		}) => void;
	}
	let {
		shuffleAnswers,
		minAnswers,
		maxAnswers,
		questionId,
		vokiId,
		cancelEditing,
		updateParent
	}: Props = $props();

	let isMultipleChoice = $state(minAnswers > 1 || maxAnswers > 1);
	let savingErrs = $state<Err[]>([]);
	async function saveChanges() {
		if (minAnswers > maxAnswers) {
			savingErrs = [
				{ message: "Minimum answers count can't be greater than maximum answers count" }
			];
			return;
		}
		savingErrs = [];

		const response = await ApiVokiCreationGeneral.fetchJsonResponse<{
			minAnswers: number;
			maxAnswers: number;
			shuffleAnswers: boolean;
		}>(
			`/vokis/${vokiId}/questions/${questionId}/update-answer-settings`,
			RJO.PATCH({
				shuffleAnswers: shuffleAnswers,
				isSingleChoice: !isMultipleChoice,
				minAnswersCountLimit: minAnswers,
				maxAnswersCountLimit: maxAnswers
			})
		);

		if (response.isSuccess) {
			updateParent({
				shuffleAnswers: response.data.shuffleAnswers,
				minAnswers: response.data.minAnswers,
				maxAnswers: response.data.maxAnswers
			});
			cancelEditing();
		} else {
			savingErrs = response.errs;
		}
	}
</script>

<div class="field">
	<VokiCreationFieldName fieldName="Answers order:" />
	<TwoStateSwitchInput
		bind:value={shuffleAnswers}
		trueLabel="Shuffled"
		trueIconId="#common-shuffle-icon"
		falseLabel="Ordered"
		falseIconId="#common-order-icon"
	/>
</div>
<div class="field">
	<VokiCreationFieldName fieldName="Selection type:" />
	<TwoStateSwitchInput
		bind:value={isMultipleChoice}
		trueLabel="Multiple"
		trueIconId="#general-voki-multiple-choice-icon"
		falseLabel="Single"
		falseIconId="#general-voki-single-choice-icon"
	/>
</div>

<div class="multiple-choice-inputs {isMultipleChoice ? 'show' : 'hide'}">
	<div class="input-field">
		<VokiCreationFieldName fieldName="Minimum answers count:" />
		<input id="min-answers-count" type="number" bind:value={minAnswers} min="1" />
	</div>
	<div class="input-field">
		<VokiCreationFieldName fieldName="	Maximum answers count:" />
		<input id="max-answers-count" type="number" bind:value={maxAnswers} />
	</div>
</div>
<DefaultErrBlock errList={savingErrs} className="question-answers-settings-err-block" />
<VokiCreationSaveAndCancelButtons onCancel={() => cancelEditing()} onSave={() => saveChanges()} />

<style>
	.field {
		display: grid;
		margin: 1.5rem 0 0;
		grid-template-columns: 11rem 1fr;
	}

	.multiple-choice-inputs {
		display: flex;
		flex-direction: column;
		gap: 1rem;
		width: 100%;
		padding: 0.25rem 0.75rem;
		margin: 1rem 0 0 1rem;
		transition: all 0.2s ease-in;
		interpolate-size: allow-keywords;
		border-left: 0.125rem solid var(--secondary-foreground);
	}

	.multiple-choice-inputs.show {
		width: 100%;
		height: auto;
		opacity: 1;
	}

	.multiple-choice-inputs.hide {
		width: 0;
		height: 0;
		opacity: 0;
		transform: translateY(-10%);
	}

	.multiple-choice-inputs * {
		interpolate-size: allow-keywords;
		transition: inherit;
	}

	.input-field {
		display: grid;
		grid-template-columns: 18rem 1fr;
	}

	.multiple-choice-inputs.hide :global(.input-field > label) {
		font-size: 1rem !important;
		transition: inherit;
	}

	.multiple-choice-inputs.hide input[type='number'] {
		font-size: 0.25rem;
		opacity: 0;
	}

	input[type='number'] {
		width: 3.75rem;
		height: 1.875rem;
		padding: 0 0.125rem;
		border: 0.125rem solid var(--secondary-foreground);
		border-radius: 0.375rem;
		background-color: var(--secondary);
		font-size: 1.25rem;
		text-align: center;
		outline: none;
		-moz-appearance: textfield;
		appearance: textfield;
	}

	input[type='number']::-webkit-outer-spin-button,
	input[type='number']::-webkit-inner-spin-button {
		appearance: none;
		margin: 0;
	}

	input[type='number']:focus {
		border-color: var(--primary);
	}

	:global(.err-block.question-answers-settings-err-block) {
		margin-top: 0.5rem;
	}
</style>
