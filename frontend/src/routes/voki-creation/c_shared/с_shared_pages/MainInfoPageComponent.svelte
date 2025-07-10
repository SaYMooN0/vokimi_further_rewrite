<script lang="ts">
	import type { VokiMainInfo } from '../../voki-creation-page-types';
	import VokiCreationCover from './c_main_info/VokiCreationCover.svelte';
	import VokiCreationDetails from './c_main_info/VokiCreationDetails.svelte';
	import VokiCreationName from './c_main_info/VokiCreationName.svelte';
	import VokiCreationTags from './c_main_info/VokiCreationTags.svelte';

	let { mainInfo }: { mainInfo: VokiMainInfo } = $props<{ mainInfo: VokiMainInfo }>();
</script>

<h1>Main info page</h1>

<div class="main-info-page-container">
	<div class="main-section">
		<VokiCreationName vokiName={mainInfo.name} />
		<VokiCreationTags tags={mainInfo.tags} />
		<VokiCreationDetails details={mainInfo.details} />
	</div>
	<div class="cover-section">
		<VokiCreationCover cover={mainInfo.cover} />
	</div>
</div>

<style>
	.main-info-page-container {
		display: grid;
		grid-template-columns: 1fr 20rem;
	}
</style>
