<script lang="ts">
	import DialogWithCloseButton from '$lib/components/dialogs/DialogWithCloseButton.svelte';

	export function open() {
        dialog.open();
	}
    let dialog = $state<DialogWithCloseButton>()!;
</script>

<DialogWithCloseButton bind:this={dialog}>
	<h1>
		You need to be logged in to take this voki
	</h1>
</DialogWithCloseButton>
