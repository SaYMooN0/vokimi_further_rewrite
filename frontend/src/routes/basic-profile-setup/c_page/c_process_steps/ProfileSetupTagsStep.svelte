<script lang="ts">
	import type { SvelteSet } from 'svelte/reactivity';

	interface Props {
		chosenTags: SvelteSet<string>;
		suggestions: Set<string>;
	}

	let { chosenTags, suggestions }: Props = $props();
</script>

<div class="chosen-tags-list">
	{#each chosenTags as tag}
		<div class="tag">
			{tag}
			<svg> <use href="#common-cross-icon" /> </svg>
		</div>
	{/each}
</div>
<h1 class="option-heder">Choose from suggestions</h1>
<div class="suggestion-list">
	{#each suggestions as tag}
		{#if !chosenTags.has(tag)}
			<div class="chosen">
				{tag}
				<svg> <use href="#common-cross-icon" /> </svg>
			</div>
		{:else}
			<div class="tag">
				{tag}
				<svg> <use href="#common-plus-icon" /> </svg>
			</div>
		{/if}
	{/each}
</div>
<p class="or-label">Or</p>
<h1 class="option-heder">Find exactly what you want</h1>
