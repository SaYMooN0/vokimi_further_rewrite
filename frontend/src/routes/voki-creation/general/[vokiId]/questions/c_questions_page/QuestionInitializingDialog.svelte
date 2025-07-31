<script lang="ts">
	import { goto } from '$app/navigation';
	import DialogWithCloseButton from '$lib/components/dialogs/DialogWithCloseButton.svelte';
	import DefaultErrBlock from '$lib/components/errs/DefaultErrBlock.svelte';
	import PrimaryButton from '$lib/components/PrimaryButton.svelte';
	import { ApiVokiCreationGeneral } from '$lib/ts/backend-communication/voki-creation-backend-service';
	import type { Err } from '$lib/ts/err';
	import { RequestJsonOptions } from '$lib/ts/request-json-options';
	import type { GeneralVokiAnswerType } from '$lib/ts/voki';
	import AnswersTypeSelectionCard from './c_initializing_dialog/AnswersTypeSelectionCard.svelte';

	const { vokiId }: { vokiId: string } = $props<{ vokiId: string }>();
	let dialog = $state<DialogWithCloseButton>()!;
	let selectedAnswersType = $state<GeneralVokiAnswerType>('TextOnly');
	let errs: Err[] = $state([]);

	export function open() {
		errs = [];
		dialog.open();
	}
	async function submitCreate() {
		const response = await ApiVokiCreationGeneral.fetchJsonResponse<{ id: string }>(
			`/vokis/${vokiId}/questions/add-new`,
			RequestJsonOptions.POST({ questionAnswersType: selectedAnswersType })
		);
		if (response.isSuccess) {
			goto(`/voki-creation/general/${vokiId}/questions/${response.data.id}`);
		} else {
			errs = response.errs;
		}
	}
</script>

<DialogWithCloseButton
	dialogId="general-voki-question-initializing-dialog"
	bind:this={dialog}
	subheading="Choose new answers type for the new question"
>
	<div class="types-container">
		<div class="type-subset-column">
			<svg><use href="#text-general-voki-answer-type-icon" /></svg>
			<div class="subset-container">
				<AnswersTypeSelectionCard
					label="Text only"
					isSelected={selectedAnswersType === 'TextOnly'}
					onClick={() => (selectedAnswersType = 'TextOnly')}
				/>
			</div>
		</div>

		<div class="columns-sep"></div>

		<div class="type-subset-column">
			<svg><use href="#color-general-voki-answer-type-icon" /></svg>
			<div class="subset-container">
				<AnswersTypeSelectionCard
					label="Color only"
					isSelected={selectedAnswersType === 'ColorOnly'}
					onClick={() => (selectedAnswersType = 'ColorOnly')}
				/>
				<AnswersTypeSelectionCard
					label="Color and Text"
					isSelected={selectedAnswersType === 'ColorAndText'}
					onClick={() => (selectedAnswersType = 'ColorAndText')}
				/>
			</div>
		</div>

		<div class="columns-sep"></div>

		<div class="type-subset-column">
			<svg><use href="#image-general-voki-answer-type-icon" /></svg>
			<div class="subset-container">
				<AnswersTypeSelectionCard
					label="Image only"
					isSelected={selectedAnswersType === 'ImageOnly'}
					onClick={() => (selectedAnswersType = 'ImageOnly')}
				/>
				<AnswersTypeSelectionCard
					label="Image and Text"
					isSelected={selectedAnswersType === 'ImageAndText'}
					onClick={() => (selectedAnswersType = 'ImageAndText')}
				/>
			</div>
		</div>

		<div class="columns-sep"></div>

		<div class="type-subset-column">
			<svg><use href="#audio-general-voki-answer-type-icon" /></svg>
			<div class="subset-container">
				<AnswersTypeSelectionCard
					label="Audio only"
					isSelected={selectedAnswersType === 'AudioOnly'}
					onClick={() => (selectedAnswersType = 'AudioOnly')}
				/>
				<AnswersTypeSelectionCard
					label="Audio and Text"
					isSelected={selectedAnswersType === 'AudioAndText'}
					onClick={() => (selectedAnswersType = 'AudioAndText')}
				/>
			</div>
		</div>
	</div>

	<DefaultErrBlock errList={errs} />

	<PrimaryButton onclick={() => submitCreate()}>Create</PrimaryButton>
</DialogWithCloseButton>

<style>
	.types-container {
		display: grid;
		grid-template-columns: 1fr auto 1fr auto 1fr auto 1fr;
		gap: 1.25rem;
		padding: 0 2rem;
	}

	.columns-sep {
		width: 0.125rem;
		height: 100%;
		border-radius: 0.125rem;
		background-color: var(--secondary);
	}

	.type-subset-column {
		display: grid;
		grid-template-rows: auto 1fr;
		gap: 2rem;
		justify-items: center;
	}

	.type-subset-column > svg {
		width: 3.25rem;
		height: 3.25rem;
		padding: 0.375rem;
		border: 0.125rem solid var(--primary);
		border-radius: 1.375rem;
		color: var(--primary);
		box-shadow: var(--shadow-md);
		stroke-width: 1.5;
	}

	.subset-container {
		display: flex;
		flex-direction: column;
		gap: 1rem;
	}

	:global(#general-voki-question-initializing-dialog .dialog-content) {
		display: flex;
		flex-direction: column;
	}

	:global(#general-voki-question-initializing-dialog .err-block) {
		margin: 1rem 0;
	}

	:global(#general-voki-question-initializing-dialog .primary-btn) {
		margin: 0 auto;
	}
</style>
