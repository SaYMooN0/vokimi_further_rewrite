<script lang="ts">
	import { StringUtils } from '$lib/ts/utils/string-utils';
	import type { Snippet } from 'svelte';

	let dialog = $state<HTMLDialogElement>()!;
	let {
		children,
		dialogId = StringUtils.rndStrWithPref('dialog-')
	}: { children: Snippet; dialogId?: string } = $props<{
		children: Snippet;
		dialogId?: string;
	}>();

	export function open() {
		dialog.showModal();
	}

	export function close() {
		dialog.close();
	}
</script>

<dialog bind:this={dialog} id={dialogId}>
	<div class="dialog-content">
		{@render children()}
	</div>
</dialog>

<style>
	dialog {
		position: fixed;
		top: 50%;
		left: 50%;
		max-width: unset;
		max-height: unset;
		padding: 0;
		border: none;
		background: none;
		transform: translate(-50%, -50%);
		outline: none;
		overflow: visible;
	}

	.dialog-content {
		position: relative;
		border-radius: 0.875rem;
		background-color: var(--back);
		box-shadow: var(--shadow);
	}
</style>
