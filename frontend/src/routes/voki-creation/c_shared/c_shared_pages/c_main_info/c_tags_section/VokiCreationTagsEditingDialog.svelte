<script lang="ts">
	import DialogWithCloseButton from '$lib/components/dialogs/DialogWithCloseButton.svelte';
	import DefaultErrBlock from '$lib/components/errs/DefaultErrBlock.svelte';
	import { ApiVokiCreationGeneral } from '$lib/ts/backend-services';
	import type { Err } from '$lib/ts/err';
	import { RequestJsonOptions } from '$lib/ts/request-json-options';

	let {
		vokiId,
		updateParent
	}: {
		vokiId: string;
		updateParent: (tags: string[]) => void;
	} = $props<{ vokiId: string; updateParent: (tags: string[]) => void }>();

	let dialogElement = $state<DialogWithCloseButton>()!;
	let errs = $state<Err[]>([]);
	let chosenTags = $state<string[]>([]);

	async function saveData() {
		const response = await ApiVokiCreationGeneral.fetchJsonResponse<{
			newTags: string[];
		}>(`/vokis/${vokiId}/update-tags`, RequestJsonOptions.PATCH({ newTags: chosenTags }));

		if (response.isSuccess) {
			updateParent(response.data.newTags);
			dialogElement.close();
		} else {
			errs = response.errs;
		}
	}
	export function open(tags: string[]) {
		chosenTags = tags;
		dialogElement.open();
	}
</script>

<DialogWithCloseButton bind:this={dialogElement} dialogId="voki-creation-tags-dialog">
	<div class="main-part">
		
	</div>
	<div class="bottom-part">
		<label class="continue-typing">
			If you don't find the tag you need continue entering the name of the tag
		</label>
		<DefaultErrBlock errList={errs} />

		<button class="save-btn" onclick={() => saveData()}>Save</button>
	</div>
</DialogWithCloseButton>

<style>
    .main-part {
        display: grid;
        grid-template-columns: 30rem 30rem;

    }
</style>