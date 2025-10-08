<script lang="ts">
	import DialogWithCloseButton from '$lib/components/dialogs/DialogWithCloseButton.svelte';

    let dialog = $state<DialogWithCloseButton>()!;
    
	export function open() {
        dialog.open();
	}
</script>

<DialogWithCloseButton bind:this={dialog} dialogId="album-creation-dialog">
	<div>if auth if not</div>
</DialogWithCloseButton>
