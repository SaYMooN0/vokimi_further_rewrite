<script lang="ts">
	import { goto } from '$app/navigation';
	import DialogWithCloseButton from '$lib/components/dialogs/DialogWithCloseButton.svelte';
	import DefaultErrBlock from '$lib/components/errs/DefaultErrBlock.svelte';
	import PrimaryButton from '$lib/components/PrimaryButton.svelte';
	import { ApiVokiCreationGeneral } from '$lib/ts/backend-communication/voki-creation-backend-service';
	import type { Err } from '$lib/ts/err';
	import { RequestJsonOptions } from '$lib/ts/request-json-options';

	const { vokiId }: { vokiId: string } = $props<{ vokiId: string }>();
	let dialog = $state<DialogWithCloseButton>()!;
	let name = $state<string>('');
	let errs: Err[] = $state([]);

	export function open() {
		errs = [];
		dialog.open();
	}
	async function submitCreate() {
		
	}
</script>

<DialogWithCloseButton
	dialogId="general-voki-result-initializing-dialog"
	bind:this={dialog}
	subheading="Enter the name of the new result"
>
	<input />
	<DefaultErrBlock errList={errs} />
	<PrimaryButton onclick={() => submitCreate()}>Create</PrimaryButton>
</DialogWithCloseButton>
