<script lang="ts">
	import MyVokisLink from './c_layout/MyVokisLink.svelte';
	import VokiInitializingDialog from './c_layout/VokiInitializingDialog.svelte';
	import type { Snippet } from 'svelte';
	import { navigating, page } from '$app/state';
	import PrimaryButton from '$lib/components/buttons/PrimaryButton.svelte';
	import CubesLoader from '$lib/components/loaders/CubesLoader.svelte';
	import AuthView from '$lib/components/AuthView.svelte';
	import MyVokiAuthNeeded from './c_layout/MyVokiAuthNeeded.svelte';
	import { AuthStore } from '$lib/ts/stores/auth-store.svelte';

	const { children }: { children: Snippet } = $props();
	let vokiInitializingDialog = $state<VokiInitializingDialog>()!;
</script>

{#snippet authStateChildren(authState: AuthStore.AuthState)}
	{#if !authState.isAuthenticated}
		<MyVokiAuthNeeded />
	{:else}
		<div class="my-vokis-page">
			<div class="links-container">
				<MyVokisLink
					content={{ text: 'Draft Vokis', isIcon: false }}
					href="/my-vokis/draft"
					isCurrent={page.data.currentTab === 'draft'}
				/>
				<MyVokisLink
					content={{ text: 'Published Vokis', isIcon: false }}
					href="/my-vokis/published"
					isCurrent={page.data.currentTab === 'published'}
				/>
				{#snippet inviteIcon()}
					<svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" fill="none">
						<path
							d="M12 7.5C12 9.433 10.433 11 8.5 11C6.567 11 5 9.433 5 7.5C5 5.567 6.567 4 8.5 4C10.433 4 12 5.567 12 7.5Z"
							stroke="currentColor"
						/>
						<path
							d="M13.5 11C15.433 11 17 9.433 17 7.5C17 5.567 15.433 4 13.5 4"
							stroke="currentColor"
							stroke-linecap="round"
						/>
						<path
							d="M13.1429 20H3.85714C2.83147 20 2 19.2325 2 18.2857C2 15.9188 4.07868 14 6.64286 14H10.3571C11.4023 14 12.3669 14.3188 13.1429 14.8568"
							stroke="currentColor"
							stroke-linecap="round"
							stroke-linejoin="round"
						/>
						<path d="M19 14V20M22 17L16 17" stroke="currentColor" stroke-linecap="round" />
					</svg>
				{/snippet}
				<MyVokisLink
					content={{ isIcon: true, icon: inviteIcon }}
					href="/my-vokis/invites"
					isCurrent={page.data.currentTab === 'invites'}
				></MyVokisLink>
			</div>
			{#if navigating.type}
				<div class="loading">
					<h1>Loading your vokis</h1>
					<CubesLoader sizeRem={5} />
				</div>
			{:else}
				<div class="my-vokis-page-content">
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
	{/if}
{/snippet}

<AuthView children={authStateChildren} />

<style>
	.loading {
		display: flex;
		flex-direction: column;
		align-items: center;
		gap: 2rem;
		margin-top: 12rem;
		animation-delay: 1s;
		animation: loading-fade-in 1s ease-in-out;
	}

	.loading h1 {
		color: var(--secondary-foreground);
		font-size: 2.5rem;
		font-weight: 600;
		letter-spacing: 2px;
	}

	.my-vokis-page {
		position: relative;
		display: grid;
		height: 100vh;
		padding-bottom: 2rem;
		grid-template-rows: auto 1fr;
		animation: loading-fade-in 0.1s ease-in-out;
	}

	.links-container {
		display: flex;
		flex-direction: row;
		justify-content: center;
		gap: 1rem;
		width: 100%;
		height: 100%;
		height: var(--sidebar-links-top-padding);
		box-sizing: border-box;
		padding: 1rem 3.5rem;
		margin: 0 auto;
		border-radius: 2rem;
		background-color: var(--back);
	}

	.my-vokis-page-content {
		overflow-y: auto;
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

	@keyframes loading-fade-in {
		0% {
			opacity: 0;
		}

		100% {
			opacity: 1;
		}
	}
</style>
