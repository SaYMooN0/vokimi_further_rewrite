<script lang="ts">
	import MyVokisLink from './c_layout/MyVokisLink.svelte';
	import VokiInitializingDialog from './c_layout/VokiInitializingDialog.svelte';
	import { type Snippet } from 'svelte';
	import { navigating, page } from '$app/state';
	import PrimaryButton from '$lib/components/buttons/PrimaryButton.svelte';
	import CubesLoader from '$lib/components/loaders/CubesLoader.svelte';
	import AuthView from '$lib/components/AuthView.svelte';
	import MyVokiAuthNeeded from './c_layout/MyVokiAuthNeeded.svelte';
	import { AuthStore } from '$lib/ts/stores/auth-store.svelte';
	import { toast } from 'svelte-sonner';
	import { setCurrentPage, type MyVokiPageApi } from './my-vokis-page-context';
	import LinesLoader from '$lib/components/loaders/LinesLoader.svelte';

	const { children }: { children: Snippet } = $props();
	let vokiInitializingDialog = $state<VokiInitializingDialog>()!;

	let currentPage: MyVokiPageApi = $state()!;
	setCurrentPage((curPage) => {
		currentPage = curPage;
	});

	async function handleRefreshClick() {
		if (!currentPage.forceRefetch) {
			toast.error('Something went wrong. Could not refresh the page');
			return;
		}
		await currentPage.forceRefetch();
	}
	let isCurrentPageLoading = $derived(currentPage ? currentPage.isLoading : true);
</script>

{#snippet authStateChildren(authState: AuthStore.AuthState)}
	{#if !authState.isAuthenticated}
		<MyVokiAuthNeeded />
	{:else}
		<div class="my-vokis-page">
			<div class="top-actions">
				<MyVokisLink
					content={{ isIcon: true, icon: inviteIcon }}
					href="/my-vokis/invites"
					isCurrent={page.data.currentTab === 'invites'}
				></MyVokisLink>

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

				<button
					onclick={() => handleRefreshClick()}
					class="force-reload-btn"
					class:btn-loading={isCurrentPageLoading}
					>{#if isCurrentPageLoading}
						<LinesLoader
							speedSec={2}
							sizeRem={1.25}
							strokePx={1.7}
							color="var(--secondary-foreground)"
						/>
					{:else}
						<svg><use href="#common-reload-icon" /></svg>
					{/if}</button
				>
			</div>
			{#if navigating.type}
				<div class="loading">
					<h1>Loading your vokis</h1>
					<CubesLoader sizeRem={5} color="var(--primary)" />
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
{#snippet inviteIcon()}
	<svg viewBox="0 0 24 24" fill="none" xmlns="http://www.w3.org/2000/svg">
		<path
			d="M2 12C2 8.22876 2 6.34315 3.17157 5.17157C4.34315 4 6.22876 4 10 4H14C17.7712 4 19.6569 4 20.8284 5.17157C22 6.34315 22 8.22876 22 12C22 15.7712 22 17.6569 20.8284 18.8284C19.6569 20 17.7712 20 14 20H10C6.22876 20 4.34315 20 3.17157 18.8284C2 17.6569 2 15.7712 2 12Z"
			stroke="currentColor"
		/>
		<path
			d="M6 8L8.1589 9.79908C9.99553 11.3296 10.9139 12.0949 12 12.0949C13.0861 12.0949 14.0045 11.3296 15.8411 9.79908L18 8"
			stroke="currentColor"
			stroke-linecap="round"
		/>
	</svg>
{/snippet}

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
		grid-template-rows: var(--sidebar-links-top-padding) 1fr;
		animation: loading-fade-in 0.1s ease-in-out;
	}

	.top-actions {
		display: grid;
		justify-content: center;
		align-items: center;
		gap: 1rem;
		width: fit-content;
		height: var(--sidebar-links-top-padding);
		box-sizing: border-box;
		padding: 1rem 0;
		margin: 0 auto;
		background-color: var(--back);
		grid-template-columns: 2rem 1fr 1fr 2rem;
	}

	.my-vokis-page-content {
		overflow-y: auto;
	}

	.force-reload-btn {
		display: grid;
		justify-content: center;
		align-items: center;
		width: 1.675rem;
		height: 1.675rem;
		margin: 0;
		border: none;
		border-radius: 100vw;
		background-color: var(--muted);
		color: var(--muted-foreground);
		box-shadow: var(--shadow-md), var(--shadow);
		cursor: pointer;
	}

	.force-reload-btn:hover {
		background-color: var(--accent);
		color: var(--accent-foreground);
	}

	.force-reload-btn svg {
		width: 1.125rem;
		height: 1.125rem;
		transition: transform 0.17s ease-out;
		stroke-width: 2;
	}

	.force-reload-btn:hover svg {
		transform: rotate(55deg);
	}

	.force-reload-btn:active svg {
		transform: scale(0.92) rotate(100deg);
		stroke-width: 2.2;
	}

	.force-reload-btn.btn-loading {
		pointer-events: none;
		opacity: 0.9;
	}

	.force-reload-btn.btn-loading > :global(*) {
		animation: fade-in-from-zero-with-delay 1s ease-in;
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
		outline: none;
	}

	:global(.my-vokis-page > .create-new-voki-btn:active) {
		transform: translateX(-50%) scaleX(0.96);
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
