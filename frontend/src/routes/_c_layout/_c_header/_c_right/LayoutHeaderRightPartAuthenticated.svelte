<script lang="ts">
	import { AuthStore } from '$lib/ts/stores/auth-store.svelte';
	import { toast } from 'svelte-sonner';
	import LogoutConfirmationDialog from './LogoutConfirmationDialog.svelte';
	import { onClickOutside } from 'runed';
	import BasicUserDisplay from '$lib/components/BasicUserDisplay.svelte';

	interface Props {
		currentUserId: string;
	}
	let { currentUserId }: Props = $props();
	let showCtxMenu = $state(false);
	let containerRef = $state<HTMLDivElement>()!;
	let logoutConfirmationDialog = $state<LogoutConfirmationDialog>()!;
	async function logoutBtnPressed() {
		if (logoutConfirmationDialog) {
			logoutConfirmationDialog.open();
		} else {
			toast.error("Logout dialog couldn't be opened");
		}
	}
	onClickOutside(
		() => containerRef,
		() => (showCtxMenu = false)
	);
</script>

<div class="authenticated" bind:this={containerRef} onclick={() => (showCtxMenu = !showCtxMenu)}>
	<BasicUserDisplay
		userId={currentUserId}
		interactionLevel="JustDisplay"
		class="user-display unselectable"
	/>
	<div class="ctx-menu" class:show={showCtxMenu}>
		<button class="logout-btn" onclick={() => logoutBtnPressed()}>Logout</button>
	</div>
	<LogoutConfirmationDialog bind:this={logoutConfirmationDialog} />
</div>

<style>
	.authenticated {
		display: grid;
		grid-template-columns: 1fr auto;
		position: relative;
		height: 100%;
		display: flex;
		margin-left: auto;
		padding: 0 0.5rem;
		border-radius: var(--radius);

		width: fit-content;
	}
	.authenticated:not(:has(.ctx-menu:hover)):hover {
		background-color: var(--muted);
	}
	.authenticated > :global(.user-display) {
		background-color: transparent;
		--profile-pic-width: 2.25rem;
	}
	.ctx-menu {
		position: absolute;
		top: calc(100% + 0.125rem);
		right: 0;
		width: 100%;
		min-width: 7rem;

		border-radius: var(--radius);
		padding: 0.25rem;
		display: none;
		background-color: var(--back);
		box-shadow: var(--shadow-xs), var(--shadow);
	}
	.ctx-menu.show {
		display: block;
	}
	.logout-btn {
		width: 100%;
		border-radius: var(--radius);
		color: var(--red-1);
		font-weight: 500;
		background-color: var(--red-3);
		border: none;
		padding: 0.25rem 1rem;
		font-size: 1rem;
	}
	.logout-btn:hover {
		background-color: var(--red-4);
	}
</style>
