<script lang="ts">
	import type { Snippet } from 'svelte';
	import GeneralVokiCreationLayoutNavBar from './c_layout/GeneralVokiCreationLayoutNavBar.svelte';
	import VokiCreationHeader from '../../c_shared/VokiCreationHeader.svelte';

	const { children }: { children: Snippet } = $props();
</script>

<VokiCreationHeader vokiName="Voki name bla bla ki name ki name " typeName="general" />
<GeneralVokiCreationLayoutNavBar />
{@render children()}
