<script lang="ts">
	import { getSignInDialogOpenFunction } from '../../ts_layout_contexts/sign-in-dialog-context';

	let { closeDialog }: { closeDialog: () => void } = $props<{ closeDialog: () => void }>();
	const openSignInDialog = getSignInDialogOpenFunction();

	function createNewAccount() {
		closeDialog();
		openSignInDialog('signup');
	}
	function login() {
		closeDialog();
		openSignInDialog('login');
	}
</script>

<div class="sign-in-required">
	<svg class="icon-1"><use href="#albums-bookmark-1-icon" /></svg>
	<svg class="icon-2"><use href="#albums-bookmark-2-icon" /></svg>
	<svg class="icon-3"><use href="#albums-clock-1-icon" /></svg>
	<svg class="icon-4"><use href="#albums-clock-2-icon" /></svg>
	<svg class="icon-5"><use href="#albums-star-1-icon" /></svg>
	<svg class="icon-6"><use href="#albums-star-2-icon" /></svg>

	<h1>Yoo need to sign in to use albums</h1>
	<div class="buttons-container">
		<button onclick={createNewAccount}>Create an account</button>
		<label onclick={login} class="login-btn">I already have an account</label>
	</div>
</div>

<style>
	h1 {
		padding: 2rem 4rem;
		white-space: nowrap;
	}

	.buttons-container {
		display: flex;
		flex-direction: column;
		gap: 0.5rem;
	}

	.login-btn {
		padding: 0.125rem 1rem;
		border-radius: 0.375rem;
		background-color: var(--back-main);
		color: var(--secondary-foreground);
		font-size: 1rem;
		text-decoration: underline;
		transition: all 0.12s ease;
		cursor: pointer;
	}

	.login-btn:hover {
		background-color: var(--secondary);
		color: var(--primary);
	}

	.login-btn:active {
		color: var(--primary-hov);
		transform: scale(0.98);
	}

	.icon-1 {
		top: 1rem;
		left: 1rem;

		--icon-color-1: var(--primary);
		--icon-color-2: var(--primary);
	}

	.icon-2 {
		top: 4rem;
		left: 1rem;

		--icon-color-2: #f15bb5;
		--icon-color-1: #9b5de5;
	}

	.icon-3 {
		top: 8rem;
		right: 6rem;

		--icon-color-1: #bc8a5f;
		--icon-color-2: #6f4518;
	}

	.icon-4 {
		top: 8rem;
		right: 1rem;

		--icon-color-1: #293241;
		--icon-color-2: #ee6c4d;
	}

	.icon-5 {
		top: 1rem;
		right: 6rem;

		--icon-color-1: var(--primary-hov);
		--icon-color-2: var(--warn-foreground);
	}

	.icon-6 {
		top: 1rem;
		right: 1rem;

		--icon-color-2: #0077b6;
		--icon-color-1: #48cae4;
	}

	svg {
		position: absolute;
		width: 2rem;
		height: 2rem;
		stroke-width: 1.5;
	}
</style>
