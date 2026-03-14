<script lang="ts">
	import type { PossiblyHidden } from '../../types';
	import ProfileSidebarSectionContainer from './_c_shared/ProfileSidebarSectionContainer.svelte';

	interface Props {
		favouriteAuthorIds: Extract<PossiblyHidden<string[]>, { showOnProfile: true }>;
	}

	let { favouriteAuthorIds }: Props = $props();
</script>

<ProfileSidebarSectionContainer title="Favourite authors">
	{#each favouriteAuthorIds.value as authorId}
		<p>{authorId}</p>
	{/each}
</ProfileSidebarSectionContainer>

<style>
</style>
