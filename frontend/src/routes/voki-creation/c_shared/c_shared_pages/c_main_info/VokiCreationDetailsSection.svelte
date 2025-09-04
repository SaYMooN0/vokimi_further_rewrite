<script lang="ts">
	import type { VokiDetails } from '$lib/ts/backend-communication/voki-creation-backend-service';
	import { LanguageUtils } from '$lib/ts/language';
	import { StringUtils } from '$lib/ts/utils/string-utils';
	import FieldNotSetLabel from '../../../../../lib/components/FieldNotSetLabel.svelte';
	import VokiCreationFieldName from '../../VokiCreationFieldName.svelte';
	import DetailsSectionEditState from './c_details_section/DetailsSectionEditState.svelte';
	import VokiCreationDefaultButton from '../../VokiCreationDefaultButton.svelte';

	let { details, vokiId }: { details: VokiDetails; vokiId: string } = $props<{
		details: VokiDetails;
		vokiId: string;
	}>();
	let isEditing = $state(false);
</script>

<div class="voki-details-section">
	{#if isEditing}
		<DetailsSectionEditState
			{vokiId}
			{details}
			updateParent={(newDetails) => (details = newDetails)}
			cancelEditing={() => (isEditing = false)}
		/>
	{:else}
		<p class="field">
			<VokiCreationFieldName fieldName="Description:" />
			{#if StringUtils.isNullOrWhiteSpace(details.description)}
				<FieldNotSetLabel text="No description" className="no-description" />
			{:else}
				{details.description}
			{/if}
		</p>
		<p class="field">
			<VokiCreationFieldName fieldName="Language:" />
			{LanguageUtils.name(details.language)}
		</p>
		<p class="field">
			<VokiCreationFieldName fieldName="Age restriction:" />
			{#if details.isAgeRestricted}
				Age restricted
			{:else}
				No age restriction
			{/if}
		</p>
		<VokiCreationDefaultButton text="Edit details" onclick={() => (isEditing = true)} />
	{/if}
</div>

<style>
	.voki-details-section {
		display: flex;
		flex-direction: column;
		width: 100%;
	}

	.field:not(:has(:global(.no-description))) {
		width: 100%;
		margin-top: 1rem;
		color: var(--text);
		font-size: 1.5rem;
		font-weight: 500;
		word-break: normal;
		overflow-wrap: anywhere;

	}

	.field:has(:global(.no-description)) {
		display: flex;
		flex-direction: row;
		align-items: center;
	}
</style>
