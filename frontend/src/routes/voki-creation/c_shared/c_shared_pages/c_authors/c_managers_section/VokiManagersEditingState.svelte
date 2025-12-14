<script lang="ts">
	import VokiCreationSaveAndCancelButtons from "../../../VokiCreationSaveAndCancelButtons.svelte";



	interface Props {
		cancelEditing: () => void;
	}
	let { cancelEditing }: Props = $props();
	async function saveChanges() {}
</script>
three states input: All co-authors, Selected co-authors, No co-authors 
<VokiCreationSaveAndCancelButtons onCancel={cancelEditing} onSave={() => saveChanges()} />
