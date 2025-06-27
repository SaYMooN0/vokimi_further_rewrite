<script lang="ts">
	import { StringUtils } from '$lib/ts/string-utils';
	import { onClickOutside } from 'runed';
	import type { Snippet } from 'svelte';

	let dialog = $state<HTMLDialogElement>()!;
	let {
		children,
		dialogId = StringUtils.rndStrWithPref('dialog-')
	}: { children: Snippet; dialogId?: string } = $props<{
		children: Snippet;
		dialogId?: string;
	}>();
	const clickOutside = onClickOutside(
		() => dialog,
		() => {
			dialog.close();
			clickOutside.stop();
		},
		{ immediate: false }
	);

	export function open() {
		dialog.showModal();
		clickOutside.start();
	}

	export function close() {
		dialog.close();
		clickOutside.stop();
	}
</script>

<dialog bind:this={dialog}>
	<div class="dialog-content" id={dialogId}>
		{@render children()}
	</div>
</dialog>

<style>
	dialog {
		max-width: unset;
		max-height: unset;
		padding: 0;
		outline: none;
		border: none;
		background: none;
		position: fixed;
		top: 50%;
		left: 50%;
		transform: translate(-50%, -50%);
		overflow: visible; 
	}
	.dialog-content {
		position: relative;
		background-color: var(--back);
		border-radius: 0.875rem;
		box-shadow: var(--shadow);
	}
</style>
