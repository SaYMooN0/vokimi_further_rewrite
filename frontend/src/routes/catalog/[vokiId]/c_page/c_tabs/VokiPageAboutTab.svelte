<script lang="ts">
	import AboutPageDescriptionSection from './c_about_tab/AboutPageDescriptionSection.svelte';
	import AboutPageFieldNameLabel from './c_about_tab/AboutPageFieldNameLabel.svelte';
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
	<AboutPageTagsSection {vokiId} {tags} />
	<AboutPageDescriptionSection {description} />
	<p class="publication-date">
		<AboutPageFieldNameLabel fieldName="Published on:" />{publicationDate}
	</p>
</div>

<style>
	.about-page-container {
		display: flex;
		flex-direction: column;
		gap: 0.5rem;
		
	}

	.publication-date {
		margin: 0;
	}
</style>
