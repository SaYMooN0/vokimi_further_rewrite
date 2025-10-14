<script lang="ts">
	import DialogWithCloseButton from '$lib/components/dialogs/DialogWithCloseButton.svelte';
	import DefaultErrBlock from '$lib/components/errs/DefaultErrBlock.svelte';
	import PrimaryButton from '$lib/components/buttons/PrimaryButton.svelte';
	import { ApiVokiCreationGeneral } from '$lib/ts/backend-communication/voki-creation-backend-service';
	import type { Err } from '$lib/ts/err';
	import { getVokiCreationPageApiService } from '../../../../voki-creation-page-context';
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
	const vokiCreationApi = getVokiCreationPageApiService();

	async function saveData() {
		const response = await vokiCreationApi.updateVokiTags(vokiId, chosenTags);
		if (response.isSuccess) {
			updateParent(response.data.newTags);
			dialogElement.close();
		} else {
			errs = response.errs;
		}
	}
	export function open(tags: string[]) {
		chosenTags = tags ?? [];
		errs = [];
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
		<TagsDialogSearchBar
			bind:searchedTags={tagsToChooseFrom}
			setErrs={(errors) => (errs = errors)}
		/>
		<label class="chosen-tags-label">
			Tags chosen ({chosenTags.length})
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
		<p class="continue-typing">
			If you don't find the tag you need continue entering the name of the tag
		</p>
		{#if errs.length > 0}
			<DefaultErrBlock errList={errs} />
		{/if}
		<PrimaryButton onclick={() => saveData()}>Save</PrimaryButton>
	</div>
</DialogWithCloseButton>

<style>
	.main-part {
		display: grid;
		place-items: center center;
		gap: 1rem;
		min-height: 20rem;
		max-height: 80vh;
		grid-template-columns: 28rem 28rem;
		grid-template-rows: 2rem 1fr;
	}

	.chosen-tags-label {
		font-size: 1.5rem;
		font-weight: 420;
	}

	.tags-ops-list {
		width: 100%;
		height: 100%;
		height: 26rem;
		scrollbar-gutter: stable;
		overflow-y: auto;
	}

	.bottom-part {
		display: flex;
		flex-direction: column;
		align-items: center;
		width: 100%;
		box-sizing: border-box;
	}

	.bottom-part > :global(.err-block) {
		margin-bottom: 1rem;
	}

	.continue-typing {
		margin: 1rem 0;
		color: var(--muted-foreground);
		font-size: 1rem;
		text-align: center;
	}
</style>
