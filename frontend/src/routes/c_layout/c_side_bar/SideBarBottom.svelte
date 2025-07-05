<script lang="ts">
	import AuthView from '$lib/components/AuthView.svelte';
	import { getSignInDialogOpenFunction } from '../../../lib/ts/contexts/sign-in-dialog';
	const openSignInDialog = getSignInDialogOpenFunction();
</script>

<AuthView {authenticated} {unauthenticated} />
{#snippet unauthenticated()}
	<div class="sign-in-btns">
		<button class="login-btn" onclick={() => openSignInDialog('login')}>Login</button>
		<button class="signup-btn" onclick={() => openSignInDialog('signup')}>Sign up</button>
	</div>
{/snippet}
{#snippet authenticated()}
	<button class="logout-btn">Logout</button>
{/snippet}

<style>
	.sign-in-btns {
		display: flex;
		flex-direction: column;
		gap: 0.5rem;
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
