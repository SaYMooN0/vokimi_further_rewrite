<script lang="ts">
	import { relativeTime } from 'svelte-relative-time';
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

<div class="about-tab-container">
	<AboutTabTagsSection {vokiId} {tags} />
	<AboutTabDescriptionSection {description} />
	<p class="publication-date">
		<AboutTabFieldNameLabel fieldName="Published:" /><span
			use:relativeTime={{
				date: publicationDate
			}}
		/>
	</p>
</div>

<style>
	.about-tab-container {
		display: flex;
		flex-direction: column;
		gap: 0.5rem;
	}

	.publication-date {
		margin: 0;
	}
</style>
