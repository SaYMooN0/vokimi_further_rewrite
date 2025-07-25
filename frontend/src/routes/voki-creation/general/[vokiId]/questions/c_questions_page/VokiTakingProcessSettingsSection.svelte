<script lang="ts">
	import VokiCreationDefaultButton from '../../../../c_shared/VokiCreationDefaultButton.svelte';
	import VokiCreationFieldName from '../../../../c_shared/VokiCreationFieldName.svelte';
	import VokiCreationSectionHeader from '../../../../c_shared/VokiCreationSectionHeader.svelte';
	import type { GeneralVokiTakingProcessSettings } from '../types';
	import TakingProcessSettingsEditingState from './c_settings_section/TakingProcessSettingsEditingState.svelte';
	import TakingProcessSettingsFieldValue from './c_settings_section/TakingProcessSettingsFieldValue.svelte';

	let { settings, vokiId }: { settings: GeneralVokiTakingProcessSettings; vokiId: string } =
		$props<{
			settings: GeneralVokiTakingProcessSettings;
			vokiId: string;
		}>();
	let isEditingState = $state(false);
</script>

<VokiCreationSectionHeader header="Voki taking process settings" />
{#if isEditingState}
	<TakingProcessSettingsEditingState
		{vokiId}
		{settings}
		updateParent={(newSettings) => (settings = newSettings)}
		cancelEditing={() => (isEditingState = false)}
	/>
{:else}
	<p class="field-p">
		<VokiCreationFieldName fieldName="Questions order:" />
		{#if settings.shuffleQuestions}
			<TakingProcessSettingsFieldValue text="Shuffled" iconId="#common-shuffle-icon" />
		{:else}
			<TakingProcessSettingsFieldValue text="Ordered" iconId="#common-order-icon" />
		{/if}
	</p>
	<p class="field-p">
		<VokiCreationFieldName fieldName="Answering flow:" />
		{#if settings.forceSequentialAnswering}
			<TakingProcessSettingsFieldValue
				text="Sequential"
				iconId="#general-voki-taking-process-settings-force-sequential-flow-icon"
			/>
		{:else}
			<TakingProcessSettingsFieldValue
				text="Free"
				iconId="#general-voki-taking-process-settings-free-flow-icon"
			/>
		{/if}
	</p>
	<VokiCreationDefaultButton text="Edit settings" onclick={() => (isEditingState = true)} />
{/if}

<style>
	.field-p {
		display: grid;
		align-items: center;
		margin: 2rem 0 0;
		grid-template-columns: 12.5rem 1fr;
	}
</style>
