<script lang="ts">
	import type { Snippet } from 'svelte';

	interface Props {
		content: { icon: Snippet; isIcon: true } | { text: string; isIcon: false };
		href: string;
		isCurrent: boolean;
	}
	let { content, href, isCurrent }: Props = $props();
</script>

<a
	data-sveltekit-preload-data="off"
	{href}
	class="unselectable"
	class:current={isCurrent}
	class:icon={content.isIcon}
	>{#if content.isIcon}{@render content!.icon!()}{:else}
		{content.text}
	{/if}</a
>

<style>
	a {
		display: flex;
		justify-content: center;
		align-items: center;
		width: 12rem;
		height: 100%;
		border-radius: 0.25rem;
		background-color: var(--muted);
		color: var(--muted-foreground);
		font-size: 1rem;
		font-weight: 430;
		text-decoration: none;
		letter-spacing: 0.2px;
		box-shadow: var(--shadow-2xl);
		transition: transform 0.12s ease;
		cursor: pointer;
	}

	a.icon {
		width: auto;
		aspect-ratio: 1/1;
	}

	a.icon > :global(svg) {
		width: 1.5rem;
		height: 1.5rem;
		transition: inherit;
		stroke-width: 1.5;
	}

	a:hover {
		background-color: var(--accent);
		color: var(--accent-foreground);
	}

	a:active {
		letter-spacing: 0;
	}

	a.icon:active > :global(svg) {
		transform: scale(0.9);
	}

	a.current {
		background-color: var(--primary);
		color: var(--primary-foreground);
		font-weight: 460;

	}

	a.current.icon > :global(svg) {
		stroke-width: 1.7;
		
	}
</style>
