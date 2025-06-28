<script lang="ts">
	import AuthView from '$lib/components/AuthView.svelte';
	import { getSignInDialogOpenFunction } from '../../sign-in-dialog';
	const openSignInDialog = getSignInDialogOpenFunction();
</script>

<AuthView {authenticated} {unauthenticated} />
{#snippet unauthenticated()}
	<div class="sign-in-btns">
		<button class="login-btn" onclick={() => openSignInDialog('login')}>Login</button>
		<button class="register-btn" onclick={() => openSignInDialog('register')}>Register</button>
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
		font-size: 1.0625rem;
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
		background-color: var(--secondary);
		color: var(--secondary-foreground);

	}

	.login-btn:hover {
		background-color: var(--accent);
		color: var(--primary);
	}

	.register-btn {
		background-color: var(--primary);
		color: var(--primary-foreground);

	}

	.register-btn:hover {
		background-color: var(--primary-hov);
	}
</style>
