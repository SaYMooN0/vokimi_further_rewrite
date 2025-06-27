<script lang="ts">
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
	async function getAuthData() {
		return { isAuthenticated: () => false };
	}
</script>

{#await getAuthData()}
	{@render loading?.()}
{:then authData}
	{#if authData !== null && authData.isAuthenticated()}
		{@render authenticated?.(authData)}
	{:else}
		{@render unauthenticated?.()}
	{/if}
{/await}
