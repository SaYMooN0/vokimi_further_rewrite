<script lang="ts">
	import DialogWithCloseButton from '$lib/components/dialogs/DialogWithCloseButton.svelte';

	let dialog = $state<DialogWithCloseButton>()!;

	export function open() {
		dialog.open();
	}
</script>

<DialogWithCloseButton
	bind:this={dialog}
	dialogId="auto-albums-editing-dialog"
	subheading="Auto albums appearance editing"
>
<h1>Editimg</h1>
</DialogWithCloseButton>
