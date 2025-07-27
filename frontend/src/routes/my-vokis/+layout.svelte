<script lang="ts">
	import MyVokisLink from './c_layout/MyVokisLink.svelte';
	import VokiInitializingDialog from './c_layout/VokiInitializingDialog.svelte';
	import type { Snippet } from 'svelte';
	import { navigating, page } from '$app/state';
	import PrimaryButton from '$lib/components/PrimaryButton.svelte';
	import AuthView from '$lib/components/AuthView.svelte';
	import PageSignInRequired from '$lib/components/PageSignInRequired.svelte';
	import CubesLoader from '$lib/components/loaders/CubesLoader.svelte';

	const { children }: { children: Snippet } = $props();
	let vokiInitializingDialog = $state<VokiInitializingDialog>()!;
</script>

<AuthView>
	{#snippet unauthenticated()}
		<PageSignInRequired />
	{/snippet}
	{#snippet authenticated()}
		<div class="my-vokis-page">
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
			{#if navigating.type}
				<div class="loading fade-in-animation">
					<h1>Loading your vokis</h1>
					<CubesLoader sizeRem={5} />
				</div>
			{:else}
				<div class="my-vokis-page-content fade-in-animation">
					{@render children()}
				</div>
			{/if}

			<PrimaryButton onclick={() => vokiInitializingDialog.open()} class="create-new-voki-btn"
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
		</div>
	{/snippet}
</AuthView>

<style>
	.loading {
		display: flex;
		flex-direction: column;
		align-items: center;
		gap: 2rem;
		margin-top: 12rem;
	}

	.loading h1 {
		color: var(--secondary-foreground);
		font-size: 2.5rem;
		font-weight: 600;
		letter-spacing: 2px;
	}

	.content {
		scrollbar-gutter: stable;
	}

	.fade-in-animation {
		animation: fade-in 0.06s ease-in-out forwards;
	}

	@keyframes fade-in {
		from {
			opacity: 0.4;
		}

		to {
			opacity: 1;
		}
	}
	.my-vokis-page {
		position: relative;
		display: grid;
		height: 100vh;
		padding-bottom: 2rem;
		grid-template-rows: auto 1fr;
	}

	.links-container {
		display: grid;
		gap: 1rem;
		width: fit-content;
		height: 100%;
		height: var(--side-bar-links-top-padding);
		box-sizing: border-box;
		padding: 1rem 3.5rem;
		margin: 0 auto;
		border-radius: 2rem;
		background-color: var(--back);
		grid-template-columns: 1fr 1fr;
	}

	.my-vokis-page-content {
		overflow-y: auto;
		scrollbar-gutter: stable;
	}

	:global(.my-vokis-page > .create-new-voki-btn) {
		position: absolute;
		bottom: 1rem;
		left: 50%;
		display: flex;
		flex-direction: row;
		align-items: center;
		gap: 0.5rem;
		padding: 0.25rem 2rem;
		border-radius: 0.375rem;
		box-shadow: var(--shadow-xl);
		transform: translateX(-50%);
	}

	:global(.my-vokis-page > .create-new-voki-btn:active) {
		transform: translateX(-50%) scale(0.96);
	}

	:global(.my-vokis-page > .create-new-voki-btn > svg) {
		height: 1.75rem;
	}
</style>
