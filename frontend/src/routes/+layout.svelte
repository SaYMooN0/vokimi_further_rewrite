<script lang="ts">
	import { type Snippet } from 'svelte';
	import SideBar from './c_layout/SideBar.svelte';
	import SignInDialog from './c_layout/SignInDialog.svelte';
	import {
		registerSignInDialogOpenFunction,
		type SignInDialogState
	} from './c_layout/c_sign_in_dialog/sign-in-dialog-context';
	import AppToaster from './c_layout/AppToaster.svelte';
	import vokiTypesIconsSprite from '$lib/icons/voki-types.svg?raw';
	import crossedCircleIconsSprite from '$lib/icons/crossed-circle.svg?raw';
	import commonIconsSprite from '$lib/icons/common-icons.svg?raw';

	let isFullWidthMode = $state(false);
	const { children }: { children: Snippet } = $props<{ children: Snippet }>();
	let signInDialog = $state<SignInDialog>()!;

	registerSignInDialogOpenFunction((state: SignInDialogState) => signInDialog.open(state));
</script>

<div class="sprites">
	{@html vokiTypesIconsSprite}
	{@html crossedCircleIconsSprite}
	{@html commonIconsSprite}
</div>
<SignInDialog bind:this={signInDialog} />
<div class="page" class:full-width={isFullWidthMode}>
	<div class="width-limit">
		<SideBar />
		<div id="page-content">
			{@render children()}
		</div>
	</div>
</div>
<AppToaster />

<style>
	.sprites {
		display: none;
		height: 0;
	}

	.page {
		display: flex;
		flex-direction: column;
		width: 100%;
		box-sizing: border-box;

		--side-bar-links-top-padding: 4rem;
		--width-limit: min(92rem, 100%);
	}

	.width-limit {
		display: grid;
		gap: 1rem;
		width: var(--width-limit);
		height: 100%;
		margin: 0 auto;
		grid-template-columns: auto 1fr;
	}

	.page.full-width {
		--width-limit: 100% !important;
	}
</style>
