<script lang="ts">
	import type { Language } from '$lib/ts/language';
	import type { AuthorLink, PossiblyHidden } from '../types';
	import SidebarAboutMeSection from './_c_sidebar/SidebarAboutMeSection.svelte';
	import SidebarLanguagesSection from './_c_sidebar/SidebarLanguagesSection.svelte';

	interface Props {
		aboutMe: PossiblyHidden<string>;
		knownLanguages: PossiblyHidden<Language[]>;
		links: PossiblyHidden<AuthorLink[]>;
		favouriteTags: PossiblyHidden<string[]>;
		favouriteAuthorIds: PossiblyHidden<string[]>;
	}

	let { links, favouriteTags, favouriteAuthorIds }: Props = $props();
	let aboutMe = {
		showOnProfile: true,
		value:
			'hello my name is gqterwg and i am a vokimi author. im here to share my knowledge and help others learn. i love to write about all sorts of things, from science and technology to history and culture. i also love to share my thoughts and ideas about the world around us. i hope you enjoy my content and learn something new. have a great day!'
	};
	let knownLanguages = {
		showOnProfile: true,
		value: ['Eng', 'Rus']
	};
</script>

<div class="author-sidebar">
	{#if aboutMe.showOnProfile}
		<SidebarAboutMeSection {aboutMe} />
	{/if}
	{#if knownLanguages.showOnProfile}
		<SidebarLanguagesSection {knownLanguages} />
	{/if}
	<!--{#if links.showOnProfile}
		<SidebarLinksSection {links} />
	{/if}
	{#if favouriteTags.showOnProfile}
		<SidebarFavouriteTagsSection {favouriteTags} />
	{/if}
	{#if favouriteAuthorIds.showOnProfile}
		<SidebarFavouriteAuthorsSection {favouriteAuthorIds} />
	{/if} -->
</div>

<style>
	.author-sidebar {
		display: flex;
		flex-direction: column;
		gap: 1rem;
	}
</style>
