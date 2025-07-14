<script lang="ts">
	import type { VokiDetails } from '$lib/ts/backend-communication/voki-creation-backend-service';
	import { LanguageUtils } from '$lib/ts/language';
	import { StringUtils } from '$lib/ts/utils/string-utils';
	import FieldNotSetLabel from '../../FieldNotSetLabel.svelte';
	import VokiCreationFieldName from '../../VokiCreationFieldName.svelte';
	import MainInfoSectionButton from './c_sections_shared/MainInfoSectionButton.svelte';

	let { details, vokiId }: { details: VokiDetails; vokiId: string } = $props<{
		details: VokiDetails;
		vokiId: string;
	}>();
	let isEditing = $state(false);
</script>

<div class="voki-details-section">
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
	<MainInfoSectionButton text="Edit details" onclick={() => (isEditing = true)} />
</div>

<style>
	.voki-details-section {
		display: flex;
		flex-direction: column;
		width: 100%;
	}
	.field {
		width: 100%;
		margin-top: 1rem;
		color: var(--text);
		font-size: 1.5rem;
		font-weight: 500;
	}
	.field:has(:global(.no-description)) {
		display: flex;
		align-items: center;
		flex-direction: row;
	}
	
</style>
