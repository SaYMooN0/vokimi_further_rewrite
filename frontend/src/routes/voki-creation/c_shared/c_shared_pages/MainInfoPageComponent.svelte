<script lang="ts">
	import type { VokiMainInfo } from '$lib/ts/backend-communication/voki-creation-backend-service';
	import VokiCreationBasicHeader from '../VokiCreationBasicHeader.svelte';
	import VokiCreationCoverSection from './c_main_info/VokiCreationCoverSection.svelte';
	import VokiCreationDetailsSection from './c_main_info/VokiCreationDetailsSection.svelte';
	import VokiCreationNameSection from './c_main_info/VokiCreationNameSection.svelte';
	import VokiCreationTagsSection from './c_main_info/VokiCreationTagsSection.svelte';

	let { mainInfo, vokiId }: { mainInfo: VokiMainInfo; vokiId: string } = $props<{
		mainInfo: VokiMainInfo;
		vokiId: string;
	}>();
</script>

<VokiCreationBasicHeader header="Voki main info" />
<div class="main-info-tab-container">
	<div class="main-section">
		<VokiCreationNameSection vokiName={mainInfo.name} {vokiId} />
		<VokiCreationTagsSection tags={mainInfo.tags} {vokiId} />
		<VokiCreationDetailsSection details={mainInfo.details} {vokiId} />
	</div>
	<div class="cover-section">
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
