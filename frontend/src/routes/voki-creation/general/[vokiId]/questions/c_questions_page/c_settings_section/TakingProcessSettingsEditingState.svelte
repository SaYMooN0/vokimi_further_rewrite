<script lang="ts">
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

	function saveChanges() {
		updateParent(settings);
		cancelEditing();
	}
</script>

<VokiCreationSaveAndCancelButtons onSave={() => saveChanges()} onCancel={cancelEditing} />
