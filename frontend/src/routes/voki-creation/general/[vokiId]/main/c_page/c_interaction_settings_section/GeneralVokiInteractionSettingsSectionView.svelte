<script lang="ts">
	import VokiCreationDefaultButton from '../../../../../c_shared/VokiCreationDefaultButton.svelte';
	import type { GeneralVokiInteractionSettings } from '../../types';

	interface Props {
		settings: GeneralVokiInteractionSettings;
		startEditing: () => void;
	}
	let { settings, startEditing }: Props = $props();
</script>

<VokiCreationDefaultButton text="Edit settings" onclick={() => startEditing()} />

<style>
</style>
