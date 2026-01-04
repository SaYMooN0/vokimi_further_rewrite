<script lang="ts">
	import { StringUtils } from '$lib/ts/utils/string-utils';
	import type { Snippet } from 'svelte';

	interface Props {
		closedby?: 'none' | 'closerequest' | 'any';
		children: Snippet;
		dialogId?: string;
	}

	let {
		closedby = 'closerequest',
		children,
		dialogId = StringUtils.rndStrWithPref('dialog-')
	}: Props = $props();

	let dialog = $state<HTMLDialogElement>()!;
	export function open() {
		dialog.showModal();
	}

	
	export function close() {
		dialog.close();
	}
</script>

<dialog bind:this={dialog} id={dialogId} {closedby}>
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
		outline: none;
		overflow: visible;
		transform-origin: top left;
		opacity: 0;
		transform: scale(1) translate(-50%, -50%);
		transition:
			opacity 0.12s ease-out,
			transform 0.12s ease-out,
			overlay 0.12s ease-out allow-discrete;
	}
	dialog:open {
		opacity: 1;
		transform: scaleY(1) translate(-50%, -50%);
	}

	@starting-style {
		dialog:open {
			opacity: 0;
			transform: scale(0.99) translate(-50%, -50%);
		}
	}
	.dialog-content {
		position: relative;
		border-radius: 0.875rem;
		background-color: var(--back);
		box-shadow: var(--shadow);
	}

	dialog::backdrop {
		background-color: transparent;
		transition:
			display 0.12s allow-discrete,
			overlay 0.12s allow-discrete,
			background-color 0.12s;
	}

	dialog:open::backdrop {
		background-color: rgb(0 0 0 / 15%);
	}

	@starting-style {
		dialog:open::backdrop {
			background-color: transparent;
		}
	}

	@media (prefers-reduced-motion: reduce) {
		dialog {
			transition: none;
			transform: translate(-50%, -50%);
			opacity: 1;
		}

		dialog::backdrop {
			transition: none;
			background-color: rgb(0 0 0 / 15%);
		}
	}
</style>
