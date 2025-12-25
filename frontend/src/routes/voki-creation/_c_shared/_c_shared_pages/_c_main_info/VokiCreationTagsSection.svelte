<script lang="ts">
	import FieldNotSetLabel from '../../../../../lib/components/FieldNotSetLabel.svelte';
	import VokiCreationFieldName from '../../VokiCreationFieldName.svelte';
	import VokiCreationDefaultButton from '../../VokiCreationDefaultButton.svelte';
	import VokiCreationTagsEditingDialog from './_c_tags_section/VokiCreationTagsEditingDialog.svelte';

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
			<FieldNotSetLabel text="No tags selected" />
		{:else}
			{#each tags as tag}
				<label class="tag">#{tag}</label>
			{/each}
		{/if}
	</p>
	<VokiCreationDefaultButton text="Edit tags" onclick={() => dialogElement.open(tags)} />
</div>

<style>
	.voki-tags-section {
		display: flex;
		flex-direction: column;
		align-items: center;
		width: 100%;
		margin-top: 1.5rem;
	}

	.tags-list {
		display: flex;
		flex-flow: row wrap;
		align-items: center;
		width: 100%;
		row-gap: 0.375rem;
	}

	.tag {
		padding: 0.125rem 0.375rem;
		margin-left: 0.375rem;
		border-radius: 0.25rem;
		background-color: var(--secondary);
		color: var(--secondary-foreground);
		font-size: 1rem;
		box-shadow: var(--shadow-xs);
	}
</style>
