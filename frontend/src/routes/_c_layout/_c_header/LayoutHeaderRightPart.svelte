<script lang="ts">
	import type { Err } from '$lib/ts/err';
	import { AuthStore } from '$lib/ts/stores/auth-store.svelte';
	import { getSignInDialogOpenFunction } from '../_ts_layout_contexts/sign-in-dialog-context';
	import LayoutHeaderRightPartAuthenticated from './_c_right/LayoutHeaderRightPartAuthenticated.svelte';
	type ViewState =
		| { name: 'authenticated'; userId: string }
		| { name: 'loading' }
		| { name: 'error'; errs: Err[] }
		| { name: 'unauthenticated' };
	const openSignInDialog = getSignInDialogOpenFunction();
	let authState = AuthStore.Get();
	let viewState: ViewState = $state(authState);
	const LOADING_DELAY_MS = 500;

	$effect(() => {
		//don't show loading state immediately so the component doesn't flash
		if (authState.name === 'loading') {
			const timer = setTimeout(() => {
				viewState = { name: 'loading' };
			}, LOADING_DELAY_MS);

			return () => {
				clearTimeout(timer);
			};
		}

		viewState = authState;
	});
	function onErrorStateReloadBtnClick() {
		if (viewState.name !== 'error') {
			return;
		}
		viewState = { name: 'loading' };
		AuthStore.GetWithForceRefresh();
	}
</script>

{#if viewState.name === 'authenticated'}
	<LayoutHeaderRightPartAuthenticated currentUserId={viewState.userId} />
{:else if viewState.name === 'loading'}
	<div class="loading">Loading...</div>
{:else if viewState.name === 'error'}
	<div class="loading-error" onclick={onErrorStateReloadBtnClick}>Authentication loading error</div>
{:else}
	<div class="sign-in-btn-container unselectable">
		<button class="sign-in-btn" onclick={() => openSignInDialog(null)}>Sign in</button>
	</div>
{/if}

<style>
	.loading-error {
		display: flex;
		justify-content: center;
		align-items: center;
		width: 100%;
		height: 100%;
		border-radius: 100vw;
		background-color: var(--red-1);
		color: var(--red-3);
		font-size: 1rem;
		font-weight: 450;
		padding: 0 0.25rem;
		letter-spacing: 0.2px;
		cursor: pointer;
		transition: all 0.06s ease-in;
		line-height: 1.1;
	}
	.loading-error:hover {
		background-color: var(--red-2);
		color: var(--red-4);
	}
	.sign-in-btn-container {
		display: flex;
		justify-content: center;
		align-items: center;
		height: 100%;
		width: 100%;
	}
	.sign-in-btn {
		border: none;
		font-size: 1.375rem;
		font-weight: 450;
		letter-spacing: 1px;
		box-shadow: var(--shadow);
		transition:
			background-color 0.06s ease-in,
			padding 0.08s ease-in;
		cursor: pointer;
		animation: fade-in-from-zero 0.12s ease-in;
		padding: 0.25rem 1.75rem;
		border-radius: var(--radius);
		color: var(--primary-foreground);
		background-color: var(--primary);
		outline: none;
	}
	.sign-in-btn:hover {
		background-color: var(--primary-hov);
	}
	.sign-in-btn:active {
		padding: 0.25rem 1.675rem;
	}

	.loading {
		height: 100%;
		animation: loading-fade-with-delay 1.5s ease-in;
		display: flex;
		justify-content: center;
		align-items: center;
		font-size: 1.125rem;
		color: var(--muted-foreground);
		font-weight: 450;
		letter-spacing: 0.25px;
	}
	@keyframes loading-fade-with-delay {
		0%,
		60% {
			opacity: 0;
		}

		100% {
			opacity: 1;
		}
	}
</style>
