<script lang="ts">
	let { children } = $props();
	import { page } from '$app/state';
	import AutoAlbumErrMessage from './c_layout/AutoAlbumErrMessage.svelte';
	import AutoAlbumPageHeader from './c_layout/AutoAlbumPageHeader.svelte';
</script>

{#if page.data.isSuccess}
	<AutoAlbumPageHeader
		albumName={page.data.data.albumName}
		vokiCount={page.data.data.vokis.length}
	/>
	{@render children()}
{:else}
	<AutoAlbumErrMessage albumName={page.data.data.albumName} errs={page.data.errs} />
{/if}
