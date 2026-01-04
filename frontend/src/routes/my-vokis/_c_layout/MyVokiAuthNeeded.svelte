<script lang="ts">
	import { AuthStore } from '$lib/ts/stores/auth-store.svelte';

	interface Props {
		currentAuthState: Exclude<AuthStore.AuthState, { isAuthenticated: true }>;
	}
	let { currentAuthState }: Props = $props();
</script>

<div>
	<h1>To create and manage Vokis you need to sign in</h1>
	{#if currentAuthState.name === 'unauthenticated'}
		<div>login and sign up buttons</div>
	{:else if currentAuthState.name === 'loading'}
		<div>Loading...</div>
	{:else if currentAuthState.name === 'error'}
		<div>Something went wrong</div>
	{:else}
		<div>Unexpected auth state</div>
	{/if}
</div>
