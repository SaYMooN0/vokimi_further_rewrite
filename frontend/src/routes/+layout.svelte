<script lang="ts">
	import { type Snippet } from 'svelte';
	import SideBar from './c_layout/SideBar.svelte';
	import SignInDialog from './c_layout/SignInDialog.svelte';
	import AppToaster from './c_layout/AppToaster.svelte';
	import vokiTypesIconsSprite from '$lib/icons/voki-type-icons.svg?raw';
	import commonIconsSprite from '$lib/icons/common-icons.svg?raw';
	import caretIconsSprite from '$lib/icons/caret-icons.svg?raw';
	import errorIconsSprite from '$lib/icons/error-icons.svg?raw';
	import ConfirmActionDialog from './c_layout/ConfirmActionDialog.svelte';
	import languagesIconsSprite from '$lib/icons/languages-icons.svg?raw';

	import {
		registerConfirmActionDialogOpenFunction,
		type ConfirmActionDialogContent
	} from './c_layout/ts_layout_contexts/confirm-action-dialog-context';
	import {
		registerSignInDialogOpenFunction,
		type SignInDialogState
	} from './c_layout/ts_layout_contexts/sign-in-dialog-context';

	let isFullWidthMode = $state(false);
	const { children }: { children: Snippet } = $props<{ children: Snippet }>();
	let signInDialog = $state<SignInDialog>()!;
	let confirmActionDialog = $state<ConfirmActionDialog>()!;

	registerSignInDialogOpenFunction((state: SignInDialogState) => signInDialog.open(state));
	registerConfirmActionDialogOpenFunction({
		open: (content: ConfirmActionDialogContent) => confirmActionDialog.open(content),
		close: () => confirmActionDialog.close()
	});
</script>

<div class="sprites">
	{@html vokiTypesIconsSprite}
	{@html commonIconsSprite}
	{@html caretIconsSprite}
	{@html errorIconsSprite}
	{@html languagesIconsSprite}
</div>
<SignInDialog bind:this={signInDialog} />
<ConfirmActionDialog bind:this={confirmActionDialog} />
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
		--width-limit: min(112rem, 100%);
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

	@media (width >= 1536px) and (width <= 1919px) {
		.page {
			--width-limit: 94vw;
		}
	}

	@media (width >= 1366px) and (width <= 1535px) {
		.page {
			--width-limit: min(80rem, 100%);
		}
	}

	@media (width <= 1365px) {
		.page {
			--width-limit: min(70rem, 98vw);
		}
	}
</style>
