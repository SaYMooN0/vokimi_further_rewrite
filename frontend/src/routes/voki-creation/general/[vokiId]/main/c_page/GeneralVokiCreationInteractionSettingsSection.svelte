<script lang="ts">
	import type { GeneralVokiInteractionSettings } from '../types';
	import GeneralVokiInteractionSettingsSectionEditing from './c_interaction_settings_section/GeneralVokiInteractionSettingsSectionEditing.svelte';
	import GeneralVokiInteractionSettingsSectionView from './c_interaction_settings_section/GeneralVokiInteractionSettingsSectionView.svelte';

	interface Props {
		settings: GeneralVokiInteractionSettings;
		vokiId: string;
	}
	let { settings: initialSettings, vokiId }: Props = $props();

	let currentInteractionSettings: GeneralVokiInteractionSettings = $state(initialSettings);
	let isEditing = $state(false);

	function startEditing() {
		isEditing = true;
	}
</script>

<div class="interaction-settings">
	{#if isEditing}
		<GeneralVokiInteractionSettingsSectionEditing
			{vokiId}
			savedInteractionSettings={currentInteractionSettings}
			cancelEditing={() => (isEditing = false)}
			updateParent={(newSettings) => {
				currentInteractionSettings = newSettings;
				isEditing = false;
			}}
		/>
	{:else}
		<GeneralVokiInteractionSettingsSectionView
			settings={currentInteractionSettings}
			startEditing={() => startEditing()}
		/>
	{/if}
</div>

<style>
	.interaction-settings {
		display: flex;
		flex-direction: column;
		width: 100%;
	}

	.interaction-settings > :global(.err-block) {
		margin: 1rem 0;
	}
</style>
