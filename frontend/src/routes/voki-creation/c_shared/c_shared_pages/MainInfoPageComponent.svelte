<script lang="ts">
	import type { VokiDetails } from '$lib/ts/backend-communication/voki-creation-backend-service';
	import type { Snippet } from 'svelte';
	import VokiCreationBasicHeader from '../VokiCreationBasicHeader.svelte';
	import VokiCreationCoverSection from './c_main_info/VokiCreationCoverSection.svelte';
	import VokiCreationDetailsSection from './c_main_info/VokiCreationDetailsSection.svelte';
	import VokiCreationNameSection from './c_main_info/VokiCreationNameSection.svelte';
	import VokiCreationTagsSection from './c_main_info/VokiCreationTagsSection.svelte';
	interface Props {
		mainInfo: {
			name: string;
			cover: string;
			tags: string[];
			details: VokiDetails;
		};
		vokiId: string;
		interactionSettingsSection: Snippet;
	}
	let { mainInfo, vokiId, interactionSettingsSection }: Props = $props();
</script>

<div class="main-info-tab-container">
	<div class="main-section">
		<VokiCreationBasicHeader header="Voki main info" />

		<VokiCreationNameSection vokiName={mainInfo.name} {vokiId} />
		<VokiCreationTagsSection tags={mainInfo.tags} {vokiId} />
		<VokiCreationDetailsSection details={mainInfo.details} {vokiId} />
		<VokiCreationBasicHeader header="Interaction settings" />

		{@render interactionSettingsSection()}
	</div>
	<div class="cover-section">
		<VokiCreationBasicHeader header="Voki cover" />

		<VokiCreationCoverSection cover={mainInfo.cover} {vokiId} />
	</div>
</div>

<style>
	.main-info-tab-container {
		display: grid;
		grid-template-columns: 1fr auto;
		gap: 1.5rem;
	}
</style>
