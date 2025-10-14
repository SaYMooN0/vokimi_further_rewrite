<script lang="ts">
	import DefaultErrBlock from '$lib/components/errs/DefaultErrBlock.svelte';
	import TwoStateSwitchInput from '$lib/components/inputs/TwoStateSwitchInput.svelte';
	import { RJO } from '$lib/ts/backend-communication/backend-services';
	import { ApiVokiCreationGeneral } from '$lib/ts/backend-communication/voki-creation-backend-service';
	import type { Err } from '$lib/ts/err';
	import VokiCreationFieldName from '../../../../../c_shared/VokiCreationFieldName.svelte';
	import VokiCreationSaveAndCancelButtons from '../../../../../c_shared/VokiCreationSaveAndCancelButtons.svelte';
	import type { GeneralVokiTakingProcessSettings } from '../../types';

	const {
		vokiId,
		settings,
		updateParent,
		cancelEditing
	}: {
		vokiId: string;
		settings: GeneralVokiTakingProcessSettings;
		updateParent: (settings: GeneralVokiTakingProcessSettings) => void;
		cancelEditing: () => void;
	} = $props<{
		vokiId: string;
		settings: GeneralVokiTakingProcessSettings;
		updateParent: (settings: GeneralVokiTakingProcessSettings) => void;
		cancelEditing: () => void;
	}>();
	let editableSettings = $state<GeneralVokiTakingProcessSettings>(settings);
	let errs = $state<Err[]>([]);
	async function saveChanges() {
		errs = [];
		const response =
			await ApiVokiCreationGeneral.fetchJsonResponse<GeneralVokiTakingProcessSettings>(
				`/vokis/${vokiId}/update-voki-taking-process-settings`,
				RJO.PATCH(editableSettings)
			);
		if (response.isSuccess) {
			updateParent(response.data);
			cancelEditing();
		} else {
			errs = response.errs;
		}
	}
</script>

<p class="field-p">
	<VokiCreationFieldName fieldName="Questions order:" />
	<TwoStateSwitchInput
		bind:value={editableSettings.shuffleQuestions}
		trueLabel="Shuffled"
		trueIconId="#common-shuffle-icon"
		falseLabel="Ordered"
		falseIconId="#common-order-icon"
	/>
</p>
<p class="field-p">
	<VokiCreationFieldName fieldName="Answering flow:" />
	<TwoStateSwitchInput
		bind:value={editableSettings.forceSequentialAnswering}
		trueLabel="Sequential"
		trueIconId="#general-voki-taking-process-settings-force-sequential-flow-icon"
		falseLabel="Free"
		falseIconId="#general-voki-taking-process-settings-free-flow-icon"
	/>
</p>
{#if errs.length > 0}
	<DefaultErrBlock errList={errs} className="voki-taking-process-settings-err-block" />
{/if}
<VokiCreationSaveAndCancelButtons onSave={() => saveChanges()} onCancel={cancelEditing} />

<style>
	.field-p {
		display: grid;
		align-items: center;
		margin: 1.5rem 0 0;
		grid-template-columns: 12rem 1fr;
	}

	:global(.err-block.voki-taking-process-settings-err-block) {
		margin-top: 0.5rem;
	}
</style>
