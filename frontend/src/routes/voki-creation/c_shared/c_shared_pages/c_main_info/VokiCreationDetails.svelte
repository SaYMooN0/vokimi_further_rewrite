<script lang="ts">
	import type { VokiDetails } from '../../../voki-creation-page-types';
	import MainInfoSectionButton from './c_sections_shared/MainInfoSectionButton.svelte';

	const { details }: { details: VokiDetails } = $props<{ details: VokiDetails }>();
	let isEditing = $state(false);
</script>

<div class="voki-details-section">
	<h1>{details.description}</h1>
	<h1>{details.language}</h1>
	<h1>{details.isAgeRestricted}</h1>
	<MainInfoSectionButton text="Edit details" onclick={() => (isEditing = true)} />
</div>

<style>
	.voki-details-section {
		display: flex;
		flex-direction: column;
		align-items: center;
		width: 100%;
	}
</style>
