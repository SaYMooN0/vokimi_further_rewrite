<script lang="ts">
	let { text, href, isCurrent }: { text: string; href: string; isCurrent: boolean } = $props<{
		text: string;
		href: string;
		isCurrent: boolean;
	}>();
</script>

<a data-sveltekit-preload-data="off" {href} class:current={isCurrent} class="unselectable">{text}</a
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
		font-weight: 450;
		text-decoration: none;
		box-shadow: var(--shadow-2xl);
		transition: transform 0.12s ease;
		cursor: pointer;
	}

	a:hover {
		background-color: var(--accent);
		color: var(--accent-foreground);
	}

	a:active {
		transform: scale(0.96);
	}

	a.current {
		background-color: var(--primary);
		color: var(--primary-foreground);
	}
</style>
