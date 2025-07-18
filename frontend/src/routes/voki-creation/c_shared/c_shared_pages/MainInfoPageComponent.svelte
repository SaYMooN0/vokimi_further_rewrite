<script lang="ts">
	import type { VokiMainInfo } from '$lib/ts/backend-communication/voki-creation-backend-service';
	import VokiCreationSectionHeader from '../VokiCreationSectionHeader.svelte';
	import VokiCreationCover from './c_main_info/VokiCreationCover.svelte';
	import VokiCreationDetails from './c_main_info/VokiCreationDetails.svelte';
	import VokiCreationName from './c_main_info/VokiCreationName.svelte';
	import VokiCreationTags from './c_main_info/VokiCreationTags.svelte';

	let { mainInfo, vokiId }: { mainInfo: VokiMainInfo; vokiId: string } = $props<{
		mainInfo: VokiMainInfo;
		vokiId: string;
	}>();
</script>

<div class="main-info-tab-container">
	<div class="main-section">
		<VokiCreationName vokiName={mainInfo.name} {vokiId} />
		<VokiCreationTags tags={mainInfo.tags} {vokiId} />
		<VokiCreationSectionHeader header="Details" />
		<VokiCreationDetails details={mainInfo.details} {vokiId} />
	</div>
	<div class="cover-section">
		<VokiCreationCover cover={mainInfo.cover} {vokiId} />
	</div>
</div>

<style>
	.main-info-tab-container {
		display: grid;
		grid-template-columns: 1fr auto;
		gap: 1.5rem;
	}
</style>
