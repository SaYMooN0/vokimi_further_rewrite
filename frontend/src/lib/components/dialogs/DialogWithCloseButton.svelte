<script lang="ts">
	import type { Snippet } from 'svelte';
	import BaseDialog from './BaseDialog.svelte';

	let { children, dialogId }: { children: Snippet; dialogId?: string } = $props<{
		children: Snippet;
		dialogId?: string;
	}>();

	let dialog = $state<BaseDialog>()!;
	export function open() {
		dialog.open();
	}
	export function close() {
		dialog.close();
	}
</script>

<BaseDialog bind:this={dialog} {dialogId}>
	<button class="dialog-close-btn" onclick={() => close()}>
		<svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" fill="none">
			<path
				d="M18 6L6.00081 17.9992M17.9992 18L6 6.00085"
				stroke="currentColor"
				stroke-width="2.2"
				stroke-linecap="round"
				stroke-linejoin="round"
			/>
		</svg>
	</button>
	{@render children()}
</BaseDialog>

<style>
	:global(dialog > .dialog-content) {
		padding: 2.5rem;
	}
	.dialog-close-btn {
		position: absolute;
		height: 1.5rem;
		width: 1.5rem;
		padding: 0.125rem;
		border: none;
		border-radius: 0.25rem;
		background-color: var(--muted);
		color: var(--muted-foreground);
		top: 0.75rem;
		right: 0.75rem;
	}
	.dialog-close-btn:hover {
		background-color: var(--secondary);
		color: var(--secondary-foreground);
	}
</style>
