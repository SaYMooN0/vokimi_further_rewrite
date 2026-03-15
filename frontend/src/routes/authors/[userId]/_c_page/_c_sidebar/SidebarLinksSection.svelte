<script lang="ts">
	import { IconUtils } from '$lib/ts/utils/icons-utils';
	import type { PossiblyHidden, AuthorLink } from '../../types';
	import ProfileSidebarSectionContainer from './_c_shared/ProfileSidebarSectionContainer.svelte';

	interface Props {
		links: Extract<PossiblyHidden<AuthorLink[]>, { showOnProfile: true }>;
	}
	let { links }: Props = $props();
</script>

<ProfileSidebarSectionContainer title="Links">
	<div class="links-list">
		{#each links.value as link}
			<a class="link" href={link.value}>
				<svg><use href={IconUtils.getProfileLinkIconIdByType(link.type)} /></svg>
				<span>{link.value}</span>
			</a>
		{/each}
	</div>
</ProfileSidebarSectionContainer>

<style>
	.links-list {
		display: flex;
		flex-direction: column;
		gap: 0.5rem;
	}
	.link {
		display: flex;
		align-items: center;
		gap: 0.5rem;
	}
	.link > svg {
		height: 2rem !important;
		width: 2rem !important;
		display: block !important;
		opacity: 1;
	}
</style>
