<script lang="ts">
	import DialogWithCloseButton from '$lib/components/dialogs/DialogWithCloseButton.svelte';
	import type { Snippet } from 'svelte';
	import type {
		ConfirmActionDialogButtons,
		ConfirmActionDialogContent,
		ConfirmActionDialogMainContent
	} from './ts_layout_contexts/confirm-action-dialog-context';

	let content = $state<ConfirmActionDialogMainContent | Snippet>();
	let buttons = $state<ConfirmActionDialogButtons | Snippet>();

	function isContentText(
		val: ConfirmActionDialogMainContent | Snippet | undefined
	): val is ConfirmActionDialogMainContent {
		return (val && 'mainText' in val) ?? false;
	}
	function isDialogButtons(
		val: ConfirmActionDialogButtons | Snippet | undefined
	): val is ConfirmActionDialogButtons {
		return (val && 'confirmBtnText' in val && 'confirmBtnOnclick' in val) ?? false;
	}
	let dialog = $state<DialogWithCloseButton>()!;

	export function open(dialogContent: ConfirmActionDialogContent) {
		content = dialogContent.mainContent;
		buttons = dialogContent.dialogButtons;
		dialog.open();
	}
</script>

<DialogWithCloseButton
	bind:this={dialog}
	dialogId="confirm-action-dialog"
	subheading={isContentText(content) ? content.subheading : undefined}
>
	{#if isContentText(content)}
		<div class="text">{content.mainText}</div>
	{:else if content}
		{@render content()}
	{/if}

	{#if isDialogButtons(buttons)}
		<div class="buttons">
			<button class="confirm" onclick={buttons.confirmBtnOnclick}>{buttons.confirmBtnText}</button>
			<button class="cancel" onclick={buttons.cancelBtnOnclick}>{buttons.cancelBtnText}</button>
		</div>
	{:else if buttons}
		{@render buttons()}
	{/if}
</DialogWithCloseButton>

<style>
	.buttons {
		align-self: end;
		margin: auto 2rem 2rem auto;
		display: grid;
		grid-template-columns: 1fr 1fr;
		gap: 0.5rem;
	}
	.text {
		text-align: center;
		font-size: 2.125rem;
	}
	.buttons button {
		padding: 8px 20px;
		border-radius: 6px;
		outline: none;
		border: none;
		color: var(--back-main);
		font-size: 20px;
		text-align: center;
		cursor: pointer;
		transition: all 0.08s ease-in;
	}

	.confirm {
		background-color: var(--primary);
		color: var(--primary-foreground);
	}
	.confirm:hover {
		background-color: var(--primary-hov);
	}
	.cancel {
		background-color: var(--muted);
		color: var(--muted-foreground);
	}
	.cancel:hover {
		background-color: var(--accent);
		color: var(--accent-foreground);
	}
</style>
