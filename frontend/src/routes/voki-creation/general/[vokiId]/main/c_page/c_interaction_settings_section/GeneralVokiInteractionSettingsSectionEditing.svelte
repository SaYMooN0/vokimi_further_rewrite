<script lang="ts">
	import DefaultErrBlock from '$lib/components/errs/DefaultErrBlock.svelte';
	import DefaultCheckBox from '$lib/components/inputs/DefaultCheckBox.svelte';
	import { RJO } from '$lib/ts/backend-communication/backend-services';
	import { ApiVokiCreationGeneral } from '$lib/ts/backend-communication/voki-creation-backend-service';
	import type { Err } from '$lib/ts/err';
	import VokiCreationFieldName from '../../../../../c_shared/VokiCreationFieldName.svelte';
	import VokiCreationSaveAndCancelButtons from '../../../../../c_shared/VokiCreationSaveAndCancelButtons.svelte';
	import type { GeneralVokiInteractionSettings } from '../../types';
	import GeneralVokiCreationResultsVisibilityInput from './c_editing/GeneralVokiCreationResultsVisibilityInput.svelte';

	interface Props {
		vokiId: string;
		savedInteractionSettings: GeneralVokiInteractionSettings;
		cancelEditing: () => void;
		updateParent: (newSettings: GeneralVokiInteractionSettings) => void;
	}
	let { vokiId, savedInteractionSettings, cancelEditing, updateParent }: Props = $props();
	let editingSettingsState = $state(savedInteractionSettings);
	let savingErrs: Err[] = $state([]);
	async function saveChanges() {
		const response = await ApiVokiCreationGeneral.fetchJsonResponse<GeneralVokiInteractionSettings>(
			`/vokis/${vokiId}/update-interaction-settings`,
			RJO.PATCH({
				signedInOnlyTaking: editingSettingsState.signedInOnlyTaking,
				resultsVisibility: editingSettingsState.resultsVisibility,
				showResultsDistribution: editingSettingsState.showResultsDistribution
			})
		);
		if (response.isSuccess) {
			updateParent(response.data);
			savingErrs = [];
			cancelEditing();
		} else {
			savingErrs = response.errs;
		}
	}
</script>

<p class="field-p">
	<VokiCreationFieldName fieldName="Signed in only taking:" />
	<DefaultCheckBox bind:checked={editingSettingsState.signedInOnlyTaking} />
</p>
<p class="field-p">
	<VokiCreationFieldName fieldName="Results visibility:" />
	<GeneralVokiCreationResultsVisibilityInput bind:value={editingSettingsState.resultsVisibility} />
</p>
<p class="field-p">
	<VokiCreationFieldName fieldName="Show results distribution:" />
	<DefaultCheckBox bind:checked={editingSettingsState.showResultsDistribution} />
</p>
<DefaultErrBlock errList={savingErrs} />
<VokiCreationSaveAndCancelButtons onCancel={cancelEditing} onSave={() => saveChanges()} />

<style>
	.field-p {
		display: flex;
		flex-direction: row;
		align-items: center;
		gap: 0.5rem;
		margin-top: 1rem;
	}
</style>
