<script lang="ts">
	import { relativeTime } from 'svelte-relative-time';
	import AboutTabDescriptionSection from './c_about_tab/AboutTabDescriptionView.svelte';
	import AboutTabTagsSection from './c_about_tab/AboutTabTagsList.svelte';
	import VokiPageTabSectionLabel from './c_tabs_shared/VokiPageTabSectionLabel.svelte';

	interface Props {
		vokiId: string;
		tags: string[];
		description: string;
		publicationDate: Date;
	}
	let { vokiId, tags, description, publicationDate }: Props = $props();
</script>

<AboutTabTagsSection {vokiId} {tags} />
<AboutTabDescriptionSection {description} />
<p class="publication-date">
	<VokiPageTabSectionLabel fieldName="Published:" /><span
		class="value"
		use:relativeTime={{
			date: publicationDate
		}}
	/>
</p>

<style>
	.publication-date {
		margin: 0;
	}

	.publication-date > .value {
		margin-left: 0.5rem;
		font-size: 1.125rem;
		font-weight: 500;
	}
</style>
