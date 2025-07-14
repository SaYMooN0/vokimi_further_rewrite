<script lang="ts">
	import FieldNotSetLabel from '../../FieldNotSetLabel.svelte';
	import VokiCreationFieldName from '../../VokiCreationFieldName.svelte';
	import MainInfoSectionButton from './c_sections_shared/MainInfoSectionButton.svelte';
	import VokiCreationTagsEditingDialog from './c_tags_section/VokiCreationTagsEditingDialog.svelte';

	let { tags, vokiId }: { tags: string[]; vokiId: string } = $props<{
		tags: string[];
		vokiId: string;
	}>();
	let dialogElement = $state<VokiCreationTagsEditingDialog>()!;
</script>

<VokiCreationTagsEditingDialog
	bind:this={dialogElement}
	{vokiId}
	updateParent={(newTags) => (tags = newTags)}
/>
<div class="voki-tags-section">
	<p class="tags-list">
		<VokiCreationFieldName fieldName="Tags:" />
		{#if tags.length === 0}
			<FieldNotSetLabel text="No tags selected" className="no-tags" />
		{:else}
			{#each tags as tag}
				<label class="tag">{tag}</label>
			{/each}
		{/if}
	</p>
	<MainInfoSectionButton text="Edit tags" onclick={() => dialogElement.open(tags)} />
</div>

<style>
	.voki-tags-section {
		display: flex;
		flex-direction: column;
		align-items: center;
		width: 100%;
	}
	.tags-list {
		width: 100%;
	}
	.tags-list:has(:global(.no-tags)) {
		display: flex;
		align-items: center;
		flex-direction: row;
	}
</style>
