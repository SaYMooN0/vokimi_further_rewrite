<script lang="ts">
	import type { Language } from '$lib/ts/language';

	interface Props {
		knownLanguages: Extract<PossiblyHidden<Language[]>, { showOnProfile: true }>;
	}

	let { knownLanguages }: Props = $props();
</script>

<ProfileSidebarSectionContainer title="Known languages">
	{#each knownLanguages.value as language}
		<p>{language.name}</p>
	{/each}
</ProfileSidebarSectionContainer>
