<script lang="ts">
	import { getSignInDialogOpenFunction } from '../../../../../../c_layout/ts_layout_contexts/sign-in-dialog-context';

	const openSignInDialog = getSignInDialogOpenFunction();
</script>

<div class="auth-needed">
	<label>To rate Vokis you need to log in</label>
	<button class="login-btn" onclick={() => openSignInDialog('login')}>Login</button>
	<button class="signup-btn" onclick={() => openSignInDialog('signup')}
		>I don't have an account yet</button
	>
</div>

<style>
	.auth-needed {
		margin: 0.25rem 1rem;
		width: calc(100% - 2rem);
		padding: 1rem;
		border-radius: 0.5rem;
		box-shadow: var(--shadow-xs);
		background-color: var(--secondary);
		display: flex;
		flex-direction: column;
		align-items: center;
        gap: 1rem;
	}
</style>
