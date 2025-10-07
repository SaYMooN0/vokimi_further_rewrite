<script lang="ts">
	import PageLoadErrView from '$lib/components/PageLoadErrView.svelte';
	import AutoAlbumsSection from './c_page/AutoAlbumsSection.svelte';
	import type { PageProps } from './$types';
	import AuthView from '$lib/components/AuthView.svelte';

	let { data }: PageProps = $props();
</script>

<AuthView>
	{#snippet unauthenticated()}
		<div class="login-required-container">
			<h1>To create, view and manage your albums you need to be logged in</h1>
		</div>
	{/snippet}
	{#snippet authenticated()}
		{#if !data.isSuccess}
			<PageLoadErrView errs={data.errs} defaultMessage="Could not load your albums" />
		{:else}
			<AutoAlbumsSection
				takenVokisAlbumsColor={data.data.takenVokisAlbums}
				ratedVokisAlbumsColor={data.data.ratedVokisAlbums}
				commentedVokisAlbumsColor={data.data.commentedVokisAlbums}
			/>
		{/if}
	{/snippet}
</AuthView>

<style>
	.login-required-container {
		width: fit-content;
		margin: 4rem auto 1rem;
	}
</style>
