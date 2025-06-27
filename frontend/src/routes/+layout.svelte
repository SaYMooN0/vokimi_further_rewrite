<script lang="ts">
	import { type Snippet } from 'svelte';
	import SideBar from './c_layout/SideBar.svelte';
	import SignInDialog from './c_layout/SignInDialog.svelte';
	import { registerSignInDialogOpenFunction, type SignInDialogState } from './sign-in-dialog';

	let isFullWidthMode = $state(false);
	const { children }: { children: Snippet } = $props<{ children: Snippet }>();
	let signInDialog = $state<SignInDialog>()!;

	registerSignInDialogOpenFunction((state: SignInDialogState) => signInDialog.open(state));
</script>

<SignInDialog bind:this={signInDialog} />
<div class="page" class:full-width={isFullWidthMode}>
	<div class="width-limit">
		<SideBar />
		<div class="page-content">
			{@render children()}
		</div>
	</div>
</div>

<style>
	.page {
		display: flex;
		flex-direction: column;
		width: 100%;

		--width-limit: min(74rem, 100%);
	}

	.width-limit {
		display: grid;
		width: var(--width-limit);
		margin: 0 auto;
		grid-template-columns: auto 1fr;
	}

	.page.full-width {
		--width-limit: 100% !important;
	}
</style>
