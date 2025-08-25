<script lang="ts">
	import AboutPageDescriptionSection from './c_about_tab/AboutPageDescriptionSection.svelte';
	import AboutPageTagsSection from './c_about_tab/AboutPageTagsSection.svelte';

	interface Props {
		vokiId: string;
		tags: string[];
		description: string;
		publicationDate: Date;
	}
	let { vokiId, tags, description, publicationDate }: Props = $props();
</script>

<div class="about-page-container">
	<AboutPageDescriptionSection {description} />
	<AboutPageTagsSection {vokiId} {tags} />
	<p>Published on {publicationDate}</p>
</div>
