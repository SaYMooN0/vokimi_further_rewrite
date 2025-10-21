<script lang="ts">
	interface Props {
		tag: string;
		isLink?: boolean;
		className?: string;
	}
	let { tag, isLink = false, className = '' }: Props = $props();
</script>

{#if isLink}
	<a href="/tags/{tag}" class="tag-chip {className}">#{tag}</a>
{:else}
	<label class="tag-chip {className}">#{tag}</label>
{/if}

<style>
	.tag-chip {
		padding: 0.125rem 0.375rem;
		background-color: var(--secondary);
		color: var(--secondary-foreground);
		box-shadow: var(--shadow-xs);
		border-radius: 0.25rem;
        	font-size: 1rem;

	}
</style>
