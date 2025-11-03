<script lang="ts">
	import { type Snippet } from 'svelte';
	import SideBar from './c_layout/SideBar.svelte';
	import AppToaster from './c_layout/AppToaster.svelte';
	import LayoutSprites from './c_layout/LayoutSprites.svelte';
	import ConfirmActionDialog from './c_layout/c_dialogs/ConfirmActionDialog.svelte';
	import SignInDialog from './c_layout/c_dialogs/SignInDialog.svelte';
	import { registerConfirmActionDialogOpenFunction } from './c_layout/ts_layout_contexts/confirm-action-dialog-context';
	import { registerSignInDialogOpenFunction } from './c_layout/ts_layout_contexts/sign-in-dialog-context';
	import VokiItemFlagsInfoDialog from './c_layout/c_dialogs/VokiItemFlagsInfoDialog.svelte';
	import { registerVokiFlagsInfoDialogOpenFunction } from './c_layout/ts_layout_contexts/voki-flags-info-dialog-context';
	import CreateNewAlbumDialog from './c_layout/c_dialogs/CreateNewAlbumDialog.svelte';
	import { registerCreateNewAlbumOpenFunction } from './c_layout/ts_layout_contexts/album-creation-dialog-context';

	let isFullWidthMode = $state(false);
	let { children }: { children: Snippet } = $props<{ children: Snippet }>();

	let signInDialog = $state<SignInDialog>()!;
	let confirmActionDialog = $state<ConfirmActionDialog>()!;
	let createNewAlbumDialog = $state<CreateNewAlbumDialog>()!;
	let vokiItemFlagsInfoDialog = $state<VokiItemFlagsInfoDialog>()!;

	registerSignInDialogOpenFunction((state) => signInDialog.open(state));
	registerConfirmActionDialogOpenFunction({
		open: (content) => confirmActionDialog.open(content),
		close: () => confirmActionDialog.close()
	});
	registerVokiFlagsInfoDialogOpenFunction(() => vokiItemFlagsInfoDialog.open());
	registerCreateNewAlbumOpenFunction((onAfterNewAlbumCreated) =>
		createNewAlbumDialog.open(onAfterNewAlbumCreated)
	);
</script>

<SignInDialog bind:this={signInDialog} />
<ConfirmActionDialog bind:this={confirmActionDialog} />
<CreateNewAlbumDialog bind:this={createNewAlbumDialog} />
<VokiItemFlagsInfoDialog bind:this={vokiItemFlagsInfoDialog} />

<LayoutSprites />
<AppToaster />

<div class="page" class:full-width={isFullWidthMode}>
	<div class="width-limit">
		<SideBar />
		<div id="page-content">
			{@render children()}
		</div>
	</div>
</div>

<style>
	.page {
		display: flex;
		flex-direction: column;
		width: 100%;
		box-sizing: border-box;

		--width-limit: 90vw;
		--sidebar-width: 13rem;
	}

	.width-limit {
		display: grid;
		gap: 1rem;
		width: var(--width-limit);
		height: 100%;
		margin: 0 auto;
		grid-template-columns: auto 1fr;
	}

	#page-content > :global(*) {
		animation: 0.2s page-fade-in;
	}

	@keyframes page-fade-in {
		from {
			opacity: 0.2;
		}

		to {
			opacity: 1;
		}
	}

	@media (1920px <= width) {
		.page {
			--width-limit: 112rem;
		}
	}

	@media (1536px <= width <= 1919px) {
		.page {
			--width-limit: 94vw;
		}
	}

	@media (1366px <= width <= 1535px) {
		.page {
			--width-limit: calc(78rem + 8vw);
		}
	}

	@media (width <= 1365px) {
		.page {
			--width-limit: 90vw;
		}
	}

	.page.full-width {
		--width-limit: 100% !important;
	}
</style>
