<script lang="ts">
	import BasicUserDisplay from '$lib/components/BasicUserDisplay.svelte';
	import VokiCreationFieldName from '../../VokiCreationFieldName.svelte';
	import type { VokiExpectedManagersSetting } from './types';

	interface Props {
		isViewerPrimaryAuthor: boolean;
		viewerId: string;
		expectedManagers: VokiExpectedManagersSetting;
	}
	let { isViewerPrimaryAuthor, viewerId, expectedManagers }: Props = $props();
</script>

{#if isViewerPrimaryAuthor}
	<p>
		<VokiCreationFieldName fieldName="After publishing:" />
		{#if expectedManagers.makeAllCoAuthorsManagers}
			<label class="field-value">All co-authors will become managers</label>
		{:else}
			<label class="field-value">Chosen co-authors will become managers: </label>
			{#each expectedManagers.userIdsToBecomeManagers as managerId}
				<BasicUserDisplay userId={managerId} interactionLevel={'UniqueNameGotoOnClick'} />
			{/each}
		{/if}
	</p>
{:else}
	<p>
		<VokiCreationFieldName fieldName="After publishing:" />
		{#if expectedManagers.makeAllCoAuthorsManagers}
			<label class="field-value">All co-authors will become managers</label>
		{:else if expectedManagers.userIdsToBecomeManagers.includes(viewerId)}
			<label class="field-value">You will become a manager</label>
		{:else if !expectedManagers.userIdsToBecomeManagers.includes(viewerId)}
			<label class="field-value">Primary author chose not to make you a manager</label>
		{/if}
	</p>
{/if}
