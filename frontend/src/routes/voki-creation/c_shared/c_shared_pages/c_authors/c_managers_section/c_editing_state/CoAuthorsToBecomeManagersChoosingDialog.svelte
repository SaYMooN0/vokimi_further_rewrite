<script lang="ts">
	import BasicUserDisplay from '$lib/components/BasicUserDisplay.svelte';
	import PrimaryButton from '$lib/components/buttons/PrimaryButton.svelte';
	import DialogWithCloseButton from '$lib/components/dialogs/DialogWithCloseButton.svelte';
	import DefaultCheckBox from '$lib/components/inputs/DefaultCheckBox.svelte';

	interface Props {
		allCoAuthors: string[];
		chosenCoAuthors: string[];
		choseCoAuthors: (coAuthorIds: string[]) => void;
	}
	let { allCoAuthors, chosenCoAuthors, choseCoAuthors }: Props = $props();
	let idToIsSelected: Record<string, boolean> = $state({});

	function onSubmit() {
		choseCoAuthors(Object.keys(idToIsSelected).filter((id) => idToIsSelected[id]));
	}
	export function open() {
		Object.fromEntries(allCoAuthors.map((id) => [id, chosenCoAuthors.includes(id)]));
		dialog.open();
	}
	let dialog = $state<DialogWithCloseButton>()!;
</script>

<DialogWithCloseButton
	subheading="Choose co-authors to become managers"
	dialogId="co-authors-to-become-managers-dialog"
	bind:this={dialog}
>
	<div class="co-authors">
		{#each allCoAuthors as coAuthorId}
			<div class="co-author">
				<BasicUserDisplay userId={coAuthorId} interactionLevel={'UniqueNameGotoOnClick'} />
				<DefaultCheckBox bind:checked={idToIsSelected[coAuthorId]} />
			</div>
		{/each}
	</div>
	<PrimaryButton onclick={onSubmit}>Save</PrimaryButton>
</DialogWithCloseButton>

<style>
	:global(.co-authors-to-become-managers-dialog > .dialog-content) {
		display: flex;
		flex-direction: column;
		gap: 1rem;
	}
	.co-authors {
		display: flex;
		flex-direction: column;
		gap: 0.5rem;
	}
	.co-author {
		display: flex;
		justify-content: space-between;
		align-items: center;
	}
</style>
