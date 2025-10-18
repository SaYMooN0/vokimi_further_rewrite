<script lang="ts">
	import type { Err } from '$lib/ts/err';
	import TagsStepChosenList from './c_tags_step/TagsStepChosenList.svelte';
	import TagsStepPartHeader from './c_tags_step/TagsStepPartHeader.svelte';
	import TagsStepSearchingColumn from './c_tags_step/TagsStepSearchingColumn.svelte';
	import TagsStepSuggestionsColumn from './c_tags_step/TagsStepSuggestionsColumn.svelte';

	interface Props {
		chosenTags: Set<string>;
		tagsSuggestionsState:
			| { name: 'loading' }
			| { name: 'ok'; tags: Iterable<string> }
			| { name: 'errs'; errs: Err[] };
		chooseTag: (tag: string) => void;
		removeTag: (tag: string) => void;
		maxTagLength: number;
	}

	let { chosenTags, tagsSuggestionsState, chooseTag, removeTag, maxTagLength }: Props = $props();
</script>

<div class="tags-step-container">
	<div class="choosing-section-headers">
		<TagsStepPartHeader text="Choose from suggestions" />
		<p class="or-label">Or</p>
		<TagsStepPartHeader text="Find exactly the tags you want" />
	</div>
	<div class="choosing-section-contents">
		<TagsStepSuggestionsColumn
			{tagsSuggestionsState}
			{chooseTag}
			{removeTag}
			isTagChosen={(tag) => chosenTags.has(tag)}
		/>

		<TagsStepSearchingColumn
			isTagChosen={(tag) => chosenTags.has(tag)}
			{chooseTag}
			{maxTagLength}
		/>
	</div>
	<div class="chosen-tags-part">
		{#if chosenTags.size === 0}
			<div class="no-tags-msg">Chosen tags will appear here</div>
		{:else}
			<TagsStepPartHeader text="You have chosen {chosenTags.size} tags" />
			<TagsStepChosenList {chosenTags} {removeTag} />
		{/if}
	</div>
</div>

<style>
	.tags-step-container {
		display: grid;
		grid-template-rows: auto minmax(20rem, 1fr) auto;
	}
	.choosing-section-headers {
		display: grid;
		grid-template-columns: 1fr auto 1fr;
		justify-content: center;
		align-items: center;
	}
	.or-label {
		font-size: 1rem;
		color: var(--secondary-foreground);
		font-weight: 600;
	}

	.choosing-section-contents {
		display: grid;
		grid-template-columns: 1fr 1fr;
		gap: 2rem;
	}

	.chosen-tags-part {
		display: grid;
		grid-template-rows: auto 1fr;
		min-height: 4.75rem;
	}
	.no-tags-msg {
		margin: 2rem auto 0;
		width: fit-content;
		height: 100%;
		place-content: center;
		padding: 0 6rem;
		background-color: var(--secondary);
		font-size: 1.125rem;
		color: var(--secondary-foreground);
		border-radius: 0.5rem;
		font-weight: 450;
	}
</style>
