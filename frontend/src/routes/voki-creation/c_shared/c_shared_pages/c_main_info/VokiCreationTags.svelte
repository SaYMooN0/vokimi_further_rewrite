<script lang="ts">
	import MainInfoSectionButton from './c_sections_shared/MainInfoSectionButton.svelte';
	import VokiCreationTagsEditingDialog from './c_tags_section/VokiCreationTagsEditingDialog.svelte';

	const { tags }: { tags: string[] } = $props<{ tags: string[] }>();
	let dialogElement = $state<VokiCreationTagsEditingDialog>()!;
</script>

<h1>
	{#each tags as tag}
		<p>
			{tag}
		</p>
	{/each}
</h1>
<VokiCreationTagsEditingDialog bind:this={dialogElement} />
<MainInfoSectionButton text="Edit details" onclick={() => dialogElement.open()} />
