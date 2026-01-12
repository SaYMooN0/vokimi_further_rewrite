<script lang="ts">
	import type { Snippet } from 'svelte';
	import VokiCreationBasicHeader from '../VokiCreationBasicHeader.svelte';
	import VokiCreationCoverSection from './_c_main_info/VokiCreationCoverSection.svelte';
	import VokiCreationDetailsSection from './_c_main_info/VokiCreationDetailsSection.svelte';
	import VokiCreationNameSection from './_c_main_info/VokiCreationNameSection.svelte';
	import VokiCreationTagsSection from './_c_main_info/VokiCreationTagsSection.svelte';
	import type { VokiCreationMainPageState } from '../../voki-creation-main-page-state.svelte';
	interface Props {
		pageState: VokiCreationMainPageState;
		vokiId: string;
		interactionSettingsSection: Snippet;
	}
	let { pageState, vokiId, interactionSettingsSection }: Props = $props();
</script>

<div class="main-info-tab-container">
	<div class="main-section">
		<VokiCreationBasicHeader header="Voki main info" />
		<VokiCreationNameSection vokiName={pageState.currentName} {vokiId} />
		<VokiCreationTagsSection
			tags={pageState.savedTags}
			{vokiId}
			updateTagsOnSave={(newTags) => (pageState.savedTags = newTags)}
		/>
		<VokiCreationDetailsSection details={pageState.currentDetails} {vokiId} />
		<VokiCreationBasicHeader header="Interaction settings" />

		{@render interactionSettingsSection()}
	</div>
	<div class="cover-section">
		<VokiCreationBasicHeader header="Voki cover" />

		<VokiCreationCoverSection cover={pageState.currentCover} {vokiId} />
	</div>
</div>

<style>
	.main-info-tab-container {
		display: grid;
		grid-template-columns: 1fr auto;
		gap: 1.5rem;
	}
</style>
