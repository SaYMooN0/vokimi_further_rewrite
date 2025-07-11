<script lang="ts">
	import type { Snippet } from 'svelte';

	type NavBarLink = {
		icon: Snippet;
		name: string;
		href: string;
	};
	const { links }: { links: NavBarLink[] } = $props<{ links: NavBarLink[] }>();
</script>

<div class="nav-bar">
	{#each links as link}
		<a href={link.href} data-sveltekit-preload-data="off">
			{@render link.icon()}
			{link.name}
		</a>
	{/each}
</div>

<style>
	.nav-bar {
		display: flex;
		flex-direction: row;
		justify-content: center;
		gap: 3rem;
		width: 100%;
	}

	.nav-bar a {
		display: flex;
		flex-direction: row;
		align-items: center;
		padding: 0.25rem 1.5rem;
		border-radius: 2rem;
		background-color: var(--muted);
		color: var(--muted-foreground);
		font-size: 1.25rem;
		font-weight: 450;
	}

	.nav-bar a:hover {
		background-color: var(--accent);
		color: var(--primary);
	}

	.nav-bar a :global(svg) {
		height: 1.5rem;
		margin-right: 0.5rem;
	}

	.nav-bar a :global(svg path) {
		stroke-width: 2;
	}
</style>
