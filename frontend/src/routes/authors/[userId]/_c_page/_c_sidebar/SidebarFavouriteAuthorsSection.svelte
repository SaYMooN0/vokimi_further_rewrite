<script lang="ts">
	import type { PossiblyHidden } from '../../types';
	import ProfileSidebarSubheader from './_c_shared/ProfileSidebarSubheader.svelte';

	interface Props {
		favouriteAuthorIds: Extract<PossiblyHidden<string[]>, { showOnProfile: true }>;
	}

	let { favouriteAuthorIds }: Props = $props();
</script>

<div class="sidebar-about-me-section">
	<ProfileSidebarSubheader title="Favourite authors" />
	{#each favouriteAuthorIds.value as authorId}
		<p>{authorId}</p>
	{/each}
</div>

<style>
	.sidebar-about-me-section {
		display: flex;
		flex-direction: column;
		gap: 0.5rem;
	}
</style>
