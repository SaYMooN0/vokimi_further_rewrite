<script lang="ts">
	import AboutTabDescriptionSection from './c_about_tab/AboutTabDescriptionSection.svelte';
	import AboutTabFieldNameLabel from './c_about_tab/AboutTabFieldNameLabel.svelte';
	import AboutTabTagsSection from './c_about_tab/AboutTabTagsSection.svelte';

	interface Props {
		vokiId: string;
		tags: string[];
		description: string;
		publicationDate: Date;
	}
	let { vokiId, tags, description, publicationDate }: Props = $props();
</script>

<div class="about-page-container">
	<AboutTabTagsSection {vokiId} {tags} />
	<AboutTabDescriptionSection {description} />
	<p class="publication-date">
		<AboutTabFieldNameLabel fieldName="Published on:" />{publicationDate}
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
