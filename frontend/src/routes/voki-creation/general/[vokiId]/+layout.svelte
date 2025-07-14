<script lang="ts">
	import type { Snippet } from 'svelte';
	import GeneralVokiCreationLayoutNavBar from './c_layout/GeneralVokiCreationLayoutNavBar.svelte';
	import VokiCreationHeader from '../../c_layout/VokiCreationHeader.svelte';
	import { setVokiCreationPageApiService } from '../../voki-creation-page-context';

	const { children }: { children: Snippet } = $props();
	setVokiCreationPageApiService('General');
</script>

<VokiCreationHeader vokiName="Voki name bla bla ki name ki name " typeName="general" />
<GeneralVokiCreationLayoutNavBar />
{@render children()}
