<script lang="ts">
	import BasicUserDisplay from '$lib/components/BasicUserDisplay.svelte';
	import VokiCreationFieldName from '../../../VokiCreationFieldName.svelte';
	import VokiCreationOneOfMultipleChoicesInput from '../../../VokiCreationOneOfMultipleChoicesInput.svelte';
	import VokiCreationSaveAndCancelButtons from '../../../VokiCreationSaveAndCancelButtons.svelte';
	import type { VokiExpectedManagersSetting } from '../types';
	import CoAuthorsToBecomeManagersChoosingDialog from './c_editing_state/CoAuthorsToBecomeManagersChoosingDialog.svelte';

	interface Props {
		cancelEditing: () => void;
		vokiCoAuthors: string[];
		savedSelectedUserIds: string[];
		initialMakeAllManagers: boolean;
		updateParent: (setting: VokiExpectedManagersSetting) => void;
	}
	let {
		cancelEditing,
		vokiCoAuthors,
		savedSelectedUserIds,
		initialMakeAllManagers,
		updateParent
	}: Props = $props();

	type ExpectedManagersType =
		| { name: 'all' }
		| { name: 'selected'; userIds: string[] }
		| { name: 'none' };
	let expectedManagers: ExpectedManagersType = $state(
		initialMakeAllManagers
			? { name: 'all' }
			: savedSelectedUserIds.length > 0
				? { name: 'selected', userIds: savedSelectedUserIds }
				: { name: 'none' }
	);
	let dialog = $state<CoAuthorsToBecomeManagersChoosingDialog>()!;

	async function saveChanges() {}
</script>

<p class="managers-input-p">
	<VokiCreationFieldName fieldName="Managers after publishing:" />
	<VokiCreationOneOfMultipleChoicesInput
		containerClass="voki-managers-input"
		options={[
			{
				name: 'All co-authors',
				selected: expectedManagers.name === 'all',
				disabled: false,
				onclick: () => (expectedManagers = { name: 'all' })
			},
			{
				name: 'Selected co-authors',
				selected: expectedManagers.name === 'selected',
				disabled: false,
				onclick: () => (expectedManagers = { name: 'selected', userIds: vokiCoAuthors })
			},
			{
				name: 'No co-authors',
				selected: expectedManagers.name === 'none',
				disabled: false,
				onclick: () => (expectedManagers = { name: 'none' })
			}
		]}
	/>
</p>
<p class="hint-p">
	{#if expectedManagers.name === 'all'}
		All co-authors will become managers when Voki will be published.
	{:else if expectedManagers.name === 'selected'}
		Selected co-authors will become managers when Voki will be published.
	{:else if expectedManagers.name === 'none'}
		No co-authors will become managers when Voki will be published.
	{/if}
	You will always be able to change managers list in the future
</p>
{#if expectedManagers.name === 'selected'}
	<VokiCreationFieldName fieldName="Managers after publishing:" />
	<div class="co-authors-select-container">
		{#each expectedManagers.userIds as userId}
			<BasicUserDisplay {userId} interactionLevel={'UniqueNameGotoOnClick'} />
		{/each}
		<button class="action-btn" onclick={() => dialog.open()}
			>{expectedManagers.userIds.length > 0 ? 'Change' : 'Choose first co-author'}
		</button>
	</div>
{/if}
<CoAuthorsToBecomeManagersChoosingDialog
	bind:this={dialog}
	allCoAuthors={vokiCoAuthors}
	chosenCoAuthors={expectedManagers.name === 'selected' ? expectedManagers.userIds : []}
	choseCoAuthors={(coAuthorIds) => (expectedManagers = { name: 'selected', userIds: coAuthorIds })}
/>
<VokiCreationSaveAndCancelButtons onCancel={cancelEditing} onSave={() => saveChanges()} />

<style>
	.managers-input-p {
		display: grid;
		grid-template-columns: max-content 1fr;
		align-items: center;
	}
	.managers-input-p > :global(.voki-managers-input) {
		margin-left: auto;
		width: fit-content;
		gap: 3rem;
	}
	.managers-input-p > :global(.voki-managers-input > .option) {
		width: 13rem;
		padding: 0.375rem 0;
	}
	.hint-p {
		margin-top: 0.5rem;
		color: var(--secondary-foreground);
		font-weight: 425;
		font-size: 0.875rem;
		margin: 0 0;
	}
</style>
