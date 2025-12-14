<script lang="ts">
	import BasicUserDisplay from '$lib/components/BasicUserDisplay.svelte';
	import VokiCreationDefaultButton from '../../VokiCreationDefaultButton.svelte';
	import VokiCreationFieldName from '../../VokiCreationFieldName.svelte';
	import VokiManagersEditingState from './c_managers_section/VokiManagersEditingState.svelte';
	import type { VokiExpectedManagersSetting } from './types';

	interface Props {
		isViewerPrimaryAuthor: boolean;
		viewerId: string;
		expectedManagers: VokiExpectedManagersSetting;
	}
	let { isViewerPrimaryAuthor, viewerId, expectedManagers }: Props = $props();
	let isEditing = $state(false);
</script>

{#if isViewerPrimaryAuthor && isEditing}
	<VokiManagersEditingState
		cancelEditing={() => {
			isEditing = false;
		}}
	/>
{:else if isViewerPrimaryAuthor && !isEditing}
	<p>
		<VokiCreationFieldName fieldName="Voki managers:" />
		{#if expectedManagers.makeAllCoAuthorsManagers}
			<label class="field-value">All co-authors will become managers</label>
		{:else if expectedManagers.userIdsToBecomeManagers.length === 0}
			<label class="field-value">No co-authors will become managers</label>
		{:else}
			<label class="field-value"
				>Chosen({expectedManagers.userIdsToBecomeManagers}) co-authors will become managers:
			</label>
			{#each expectedManagers.userIdsToBecomeManagers as managerId}
				<BasicUserDisplay userId={managerId} interactionLevel={'UniqueNameGotoOnClick'} />
			{/each}
		{/if}
	</p>
	<VokiCreationDefaultButton text="Edit Managers" onclick={() => (isEditing = true)} />
{:else}
	<p>
		<VokiCreationFieldName fieldName="Voki managers:" />
		{#if expectedManagers.makeAllCoAuthorsManagers}
			<label class="field-value">All co-authors will become managers</label>
		{:else if expectedManagers.userIdsToBecomeManagers.includes(viewerId)}
			<label class="field-value">You will become a manager</label>
		{:else if !expectedManagers.userIdsToBecomeManagers.includes(viewerId)}
			<label class="field-value">Primary author chose not to make you a manager</label>
		{/if}
	</p>
{/if}
