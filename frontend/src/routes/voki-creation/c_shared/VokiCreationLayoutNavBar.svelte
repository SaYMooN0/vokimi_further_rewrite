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
	start
	{#each links as link}
		<a href={link.href}>{link.name}</a>
	{/each}
	end
</div>

<style>
	.nav-bar {
		display: flex;
		flex-direction: row;
		justify-content: space-between;
		width: 100%;
		background-color: darkblue;
	}
</style>
