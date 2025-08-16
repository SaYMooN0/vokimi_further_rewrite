<script lang="ts">
	import type { PageProps } from './$types';
	import VokiNotLoaded from './c_page/VokiNotLoaded.svelte';
	import languagesIconsSprite from '$lib/icons/languages-icons.svg?raw';

	let { data }: PageProps = $props();
</script>

{@html languagesIconsSprite}
{#if !data.isSuccess}
	<VokiNotLoaded errs={data.errs} vokiId={data.vokiId!} />
{:else}
	<h1>voki page {JSON.stringify(data.data)}</h1>
{/if}
