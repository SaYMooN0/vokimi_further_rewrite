<script lang="ts">
	import type { Snippet } from 'svelte';
	interface SideBarLinkProps {
		href: string;
		text: string;
		children: Snippet;
		isCollapsed: boolean;
		animateIcon: () => void;
	}
	let { href, text, children, isCollapsed, animateIcon }: SideBarLinkProps = $props();
</script>

<a
	{href}
	class:collapsed={isCollapsed}
	class="link"
	onmouseenter={(e) => animateIcon()}
	onfocus={(e) => animateIcon()}
>
	{@render children()}
	<label>{text}</label>
</a>

<style>
	.link {
		display: grid;
		align-items: center;
		gap: 0.5rem;
		padding: 0.25rem 0.5rem;
		border-radius: 0.25rem;
		background-color: var(--muted);
		font-weight: 450;
		cursor: pointer;
		grid-template-columns: 1.5rem 1fr;
		transition: transform 0.12s ease-in;
		color: var(--primary);
		outline-color: var(--primary);
		
	}

	.link:hover {
		box-shadow: var(--shadow-xs);
		background-color: var(--accent);
	}
	

	.link * {
		cursor: inherit;
	}

	.link.collapsed {
		display: flex;
	}

	.link.collapsed > label {
		display: none;
	}
</style>
