<script lang="ts">
	import VokiCreationDefaultButton from '../../../../c_shared/VokiCreationDefaultButton.svelte';
	import VokiCreationFieldName from '../../../../c_shared/VokiCreationFieldName.svelte';
	import VokiCreationSectionHeader from '../../../../c_shared/VokiCreationSectionHeader.svelte';
	import type { GeneralVokiTakingProcessSettings } from '../types';
	import TakingProcessSettingsEditingState from './c_settings_section/TakingProcessSettingsEditingState.svelte';

	let { settings, vokiId }: { settings: GeneralVokiTakingProcessSettings; vokiId: string } =
		$props<{
			settings: GeneralVokiTakingProcessSettings;
			vokiId: string;
		}>();
	let isEditingState = $state(false);
</script>

{#if isEditingState}
	<TakingProcessSettingsEditingState
		{vokiId}
		{settings}
		updateParent={(newSettings) => (settings = newSettings)}
		cancelEditing={() => (isEditingState = false)}
	/>
{:else}
	<VokiCreationSectionHeader header="Voki taking process settings" />
	<p class="field-p">
		<VokiCreationFieldName fieldName="Questions order:" />
		{settings.shuffleQuestions ? 'Shuffled' : 'Ordered'}
	</p>
	<p class="field-p">
		<VokiCreationFieldName fieldName="Answering mode:" />
		{settings.forceSequentialAnswering ? 'Sequential' : 'Free'}
	</p>
	<VokiCreationDefaultButton text="Edit settings" onclick={() => (isEditingState = true)} />
{/if}
