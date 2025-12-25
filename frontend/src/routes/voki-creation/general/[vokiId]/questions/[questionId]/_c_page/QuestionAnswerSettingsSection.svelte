<script lang="ts">
	import VokiCreationDefaultButton from '../../../../../_c_shared/VokiCreationDefaultButton.svelte';
	import VokiCreationFieldName from '../../../../../_c_shared/VokiCreationFieldName.svelte';
	import QuestionSettingsEditingState from './_c_settings_section/QuestionSettingsEditingState.svelte';
	import QuestionSettingsFieldValue from './_c_settings_section/QuestionSettingsFieldValue.svelte';

	let {
		shuffleAnswers,
		minAnswers,
		maxAnswers,
		questionId,
		vokiId
	}: {
		shuffleAnswers: boolean;
		minAnswers: number;
		maxAnswers: number;
		questionId: string;
		vokiId: string;
	} = $props<{
		shuffleAnswers: boolean;
		minAnswers: number;
		maxAnswers: number;
		questionId: string;
		vokiId: string;
	}>();
	let isSingleChoice = $derived(minAnswers === 1 && maxAnswers === 1);
	let isEditingState = $state(false);
</script>

{#if isEditingState}
	<QuestionSettingsEditingState
		{shuffleAnswers}
		{minAnswers}
		{maxAnswers}
		{questionId}
		{vokiId}
		cancelEditing={() => (isEditingState = false)}
		updateParent={(newSettings) => {
			shuffleAnswers = newSettings.shuffleAnswers;
			minAnswers = newSettings.minAnswers;
			maxAnswers = newSettings.maxAnswers;
		}}
	/>
{:else}
	<div class="field">
		<VokiCreationFieldName fieldName="Answers order:" />
		<QuestionSettingsFieldValue>
			{#if shuffleAnswers}
				Shuffled
			{:else}
				Ordered
			{/if}
		</QuestionSettingsFieldValue>
	</div>
	<div class="field">
		<VokiCreationFieldName fieldName="Selection type:" />
		<QuestionSettingsFieldValue>
			{#if isSingleChoice}
				Single choice
			{:else}
				Multiple choice (from {minAnswers} to {maxAnswers})
			{/if}
		</QuestionSettingsFieldValue>
	</div>
	<VokiCreationDefaultButton
		text="Edit answer settings"
		class="edit-settings-button"
		onclick={() => (isEditingState = true)}
	/>
{/if}

<style>
	.field {
		display: flex;
		flex-direction: row;
		gap: 0.5rem;
		margin: 1.5rem 0 0;
	}

	:global(.edit-settings-button) {
		padding: 0 1rem;
	}
</style>
