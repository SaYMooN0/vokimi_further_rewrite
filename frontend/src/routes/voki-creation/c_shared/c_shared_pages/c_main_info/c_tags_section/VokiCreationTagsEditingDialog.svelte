<script lang="ts">
	import DialogWithCloseButton from '$lib/components/dialogs/DialogWithCloseButton.svelte';
	import DefaultErrBlock from '$lib/components/errs/DefaultErrBlock.svelte';
	import { ApiVokiCreationGeneral } from '$lib/ts/backend-communication/voki-creation-backend-service';
	import type { Err } from '$lib/ts/err';
	import { RequestJsonOptions } from '$lib/ts/request-json-options';
	import TagOperatingDisplay from './TagOperatingDisplay.svelte';
	import TagsDialogSearchBar from './TagsDialogSearchBar.svelte';

	let {
		vokiId,
		updateParent
	}: {
		vokiId: string;
		updateParent: (newTags: string[]) => void;
	} = $props<{ vokiId: string; updateParent: (newTags: string[]) => void }>();

	let dialogElement = $state<DialogWithCloseButton>()!;
	let errs = $state<Err[]>([]);
	let tagsToChooseFrom: string[] = $state([]);
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
	function removeTag(tag: string) {
		chosenTags = chosenTags.filter((t) => t !== tag);
	}
	function addTag(tag: string) {
		if (!chosenTags.includes(tag)) {
			chosenTags.push(tag);
		}
	}
</script>

<DialogWithCloseButton bind:this={dialogElement} dialogId="voki-creation-tags-dialog">
	<div class="main-part">
		<TagsDialogSearchBar bind:searchedTags={tagsToChooseFrom} />
		<label class="chosen-tags-label">
			Tags chosen: ({chosenTags.length})
		</label>
		<div class="tags-ops-list">
			{#each tagsToChooseFrom as tag}
				<TagOperatingDisplay
					{tag}
					isTagAdded={chosenTags.includes(tag)}
					isTagRemovingState={false}
					btnOnClick={() => addTag(tag)}
				/>
			{/each}
		</div>

		<div class="tags-ops-list">
			{#each chosenTags as tag}
				<TagOperatingDisplay
					{tag}
					isTagAdded={true}
					isTagRemovingState={true}
					btnOnClick={() => removeTag(tag)}
				/>
			{/each}
		</div>
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
		grid-template-rows: 2rem 1fr;
		min-height: 30rem;
		max-height: 80vh;
	}
</style>
