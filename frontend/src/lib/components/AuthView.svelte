<script lang="ts">
	import { getAuthStore } from '$lib/ts/stores/auth-store.svelte';
	import type { Snippet } from 'svelte';

	type AuthStoreData = { isAuthenticated: () => boolean };
	const {
		loading = null,
		authenticated = null,
		unauthenticated = null
	} = $props<{
		loading?: Snippet;
		authenticated?: Snippet<[AuthStoreData]>;
		unauthenticated?: Snippet;
	}>();
</script>

{#await getAuthStore()}
	{@render loading?.()}
{:then authData}
	{#if authData !== null && authData.isAuthenticated}
		{@render authenticated?.(authData)}
	{:else}
		{@render unauthenticated?.()}
	{/if}
{/await}
