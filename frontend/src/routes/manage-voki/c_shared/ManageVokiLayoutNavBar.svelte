<script lang="ts">
	interface Props {
		links: {
			name: string;
			key: string;
			href: string;
		}[];
		activeLinkKey: string | undefined;
	}
	const { links, activeLinkKey }: Props = $props();
</script>

<div class="nav-bar">
	{#each links as link}
		<a
			href={link.href}
			data-sveltekit-preload-data="off"
			class="link"
			class:active={link.key === activeLinkKey}
		>
			{link.name}
		</a>
	{/each}
</div>

<style>
	.nav-bar {
		width: calc(100% - 2rem);
		margin: 1rem 1rem 0;
		box-shadow: var(--shadow-xs);
		display: grid;
		grid-template-columns: repeat(auto-fit, minmax(0, 1fr));
		gap: 1rem;
		border-radius: 100vw;
		padding: 0.25rem;
	}

	.link {
		display: block;
		width: 100%;
		padding: 0.5rem 0;
		color: var(--secondary-foreground);
		font-size: 1.125rem;
		font-weight: 450;
		text-align: center;
		text-decoration: none;
		border-radius: 100vw;
	}
	.link:not(.active):hover {
		background-color: var(--accent);
		color: var(--accent-foreground);
	}
	.link.active {
		background-color: var(--primary);
		color: var(--primary-foreground);
	}
</style>
