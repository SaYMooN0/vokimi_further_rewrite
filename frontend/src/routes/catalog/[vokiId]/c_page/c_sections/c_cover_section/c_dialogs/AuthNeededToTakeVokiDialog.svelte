<script lang="ts">
	import DialogWithCloseButton from '$lib/components/dialogs/DialogWithCloseButton.svelte';
	import { getSignInDialogOpenFunction } from '../../../../../../c_layout/ts_layout_contexts/sign-in-dialog-context';

	export function open() {
		dialog.open();
	}
	let dialog = $state<DialogWithCloseButton>()!;
	const openSignInDialog = getSignInDialogOpenFunction();
</script>

<DialogWithCloseButton bind:this={dialog} dialogId="auth-needed-to-take-voki-dialog">
	<h1 class="title">Sign in required</h1>
	<p class="message">
		You need to be logged in to take this Voki<br />
		<span class="note">It is not our wish, the Voki Author decided so</span>
	</p>

	<div class="actions">
		<button class="primary-btn" onclick={() => openSignInDialog('login')}> Login </button>
		<button class="secondary-btn" onclick={() => openSignInDialog('signup')}> Sign up </button>
	</div>
</DialogWithCloseButton>

<style>
	:global(#auth-needed-to-take-voki-dialog > .dialog-content) {
		padding: 2rem 4rem;
		color: var(--text);
		display: flex;
		flex-direction: column;
		align-items: center;
		gap: 1.25rem;
		display: flex;
		flex-direction: column;
		align-items: center;
		gap: 1.25rem;
	}
	.title {
		font-size: 2rem;
		font-weight: 600;
		color: var(--primary);
		margin: 0;
	}

	.message {
		font-size: 1em;
		color: var(--secondary-foreground);
		line-height: 1.4;
		letter-spacing: 0.25px;
		margin: 0;
	}

	.actions {
		display: flex;
		gap: 1rem;
		margin-top: 0.5rem;
	}

	.primary-btn,
	.secondary-btn {
		width: 9rem;
		padding: 0.5rem 0;
		border-radius: 0.25rem;
		font-size: 1rem;
		font-weight: 500;
		cursor: pointer;
		transition: 0.15s ease;
		border: none;

	}

	.primary-btn {
		background-color: var(--primary);
		color: var(--primary-foreground);
	}

	.primary-btn:hover {
		background-color: var(--primary-hov);
	}

	.secondary-btn {
		background-color: var(--muted);
		color: var(--muted-foreground);
	}

	.secondary-btn:hover {
		background-color: var(--accent);
		color: var(--accent-foreground);
	}
</style>
