<script lang="ts">
	import MyVokisLink from './c_layout/MyVokisLink.svelte';
	import VokiInitializingDialog from './c_layout/VokiInitializingDialog.svelte';
	import type { Snippet } from 'svelte';
	import { page } from '$app/state';
	import PrimaryButton from '$lib/components/PrimaryButton.svelte';
	import AuthView from '$lib/components/AuthView.svelte';
	import PageSignInRequired from '$lib/components/PageSignInRequired.svelte';

	const { children }: { children: Snippet } = $props();
	let vokiInitializingDialog = $state<VokiInitializingDialog>()!;
</script>

<AuthView {unauthenticated} {authenticated} />
{#snippet unauthenticated()}
	<PageSignInRequired />
{/snippet}
{#snippet authenticated()}
	<div class="links-container">
		<MyVokisLink
			text="Draft Vokis"
			href="/my-vokis/draft"
			isCurrent={page.data.currentTab === 'draft'}
		/>
		<MyVokisLink
			text="Published Vokis"
			href="/my-vokis/published"
			isCurrent={page.data.currentTab === 'published'}
		/>
	</div>
	<div class="my-vokis-page-content">
		{@render children()}
	</div>
	<PrimaryButton onclick={() => vokiInitializingDialog.open()}
		>Create new voki
		<svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" fill="none">
			<path
				d="M21.5 12C21.5 7.52166 21.5 5.28249 20.1088 3.89124C18.7175 2.5 16.4783 2.5 12 2.5C7.52166 2.5 5.28249 2.5 3.89124 3.89124C2.5 5.28249 2.5 7.52166 2.5 12C2.5 16.4783 2.5 18.7175 3.89124 20.1088C5.28249 21.5 7.52166 21.5 12 21.5C16.4783 21.5 18.7175 21.5 20.1088 20.1088C21.5 18.7175 21.5 16.4783 21.5 12Z"
				stroke="currentColor"
				stroke-width="2"
				stroke-linejoin="round"
			></path>
			<path
				d="M10 8L13.3322 11.0203C14.2226 11.8273 14.2226 12.1727 13.3322 12.9797L10 16"
				stroke="currentColor"
				stroke-width="2"
				stroke-linecap="round"
				stroke-linejoin="round"
			></path>
		</svg>
	</PrimaryButton>
	<VokiInitializingDialog bind:this={vokiInitializingDialog} />
{/snippet}

<style>
	:global(#page-content) {
		display: grid;
		grid-template-rows: 1fr calc(100vh - 5.5rem);
		padding-bottom: 2rem;
		position: relative;
	}
	.links-container {
		top: 2rem;
		display: grid;
		gap: 1rem;
		width: fit-content;
		height: 100%;
		box-sizing: border-box;
		padding: 0.75rem 3.5rem;
		margin: 0 auto;
		border-radius: 2rem;
		background-color: var(--back);
		grid-template-columns: 1fr 1fr;
	}
	.my-vokis-page-content {
		justify-self: top;
		overflow-y: auto;
	}

	:global(#page-content > .primary-btn) {
		position: absolute;
		left: 50%;
		bottom: 1rem;
		transform: translateX(-50%);
		display: flex;
		flex-direction: row;
		align-items: center;
		gap: 0.5rem;
		border-radius: 0.375rem;
		padding: 0.25rem 2rem;
		box-shadow: var(--shadow-xl);
	}
	:global(#page-content > .primary-btn > svg) {
		height: 1.75rem;
	}
</style>
