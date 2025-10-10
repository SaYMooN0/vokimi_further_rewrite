<script lang="ts">
	import ReloadButton from '$lib/components/buttons/ReloadButton.svelte';
	import { AuthStore } from '$lib/ts/stores/auth-store.svelte';
	import { getSignInDialogOpenFunction } from '../ts_layout_contexts/sign-in-dialog-context';
	const openSignInDialog = getSignInDialogOpenFunction();

	let authState = AuthStore.Get();

	let viewState: 'authenticated' | 'loading' | 'error' | 'unauthenticated' = $state(authState.name);

	const LOADING_DELAY_MS = 500;

	$effect(() => {
		//don't show loading state immediately so the component doesn't flash
		if (authState.name === 'loading') {
			const timer = setTimeout(() => {
				viewState = 'loading';
			}, LOADING_DELAY_MS);

			return () => {
				clearTimeout(timer);
			};
		}

		viewState = authState.name;
	});
	function onErrorStateReloadBtnClick() {
		if (viewState !== 'error') {
			return;
		}
		viewState = 'loading';
		AuthStore.GetWithForceRefresh();
	}
</script>

{#if viewState === 'authenticated'}
	<button class="logout-btn">Logout</button>
{:else if viewState === 'loading'}
	<div class="appear-with-delay">Loading...</div>
{:else if viewState === 'error'}
	<div class="error-container">
		<label>Authentication loading error</label>
		<ReloadButton onclick={() => onErrorStateReloadBtnClick()} showIcon={false} />
	</div>
{:else}
	<div class="sign-in-btns">
		<button class="login-btn" onclick={() => openSignInDialog('login')}>Login</button>
		<button class="signup-btn" onclick={() => openSignInDialog('signup')}>Sign up</button>
	</div>
{/if}

<style>
	.error-container {
		display: flex;
		flex-direction: column;
		justify-content: center;
		align-items: center;
		gap: 0.5rem;
	}

	.error-container > label {
		width: 100%;
		padding: 0.25rem 1rem;
		border-radius: 1rem;
		background-color: var(--err-back);
		color: var(--err-foreground);
		font-size: 1rem;
		font-weight: 500;
		text-align: center;
		letter-spacing: 0.25px;
	}

	.sign-in-btns {
		display: flex;
		flex-direction: column;
		gap: 0.5rem;
		animation: default-fade-in 0.1s ease-in;
	}

	.sign-in-btns button {
		width: 100%;
		height: 2rem;
		border: none;
		border-radius: 0.25rem;
		font-size: 1.25rem;
		font-weight: 500;
		letter-spacing: 0.5px;
		box-shadow: var(--shadow);
		transition: all 0.06s ease-in;
		cursor: pointer;
	}

	.sign-in-btns button:active {
		transform: scale(0.98);
	}

	.login-btn {
		background-color: var(--muted);
		color: var(--muted-foreground);
	}

	.login-btn:hover {
		background-color: var(--accent);
		color: var(--primary);
	}

	.signup-btn {
		background-color: var(--primary);
		color: var(--primary-foreground);
	}

	.signup-btn:hover {
		background-color: var(--primary-hov);
	}
</style>
