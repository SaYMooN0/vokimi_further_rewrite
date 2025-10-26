<script lang="ts">
	interface Props {
		albumName: string;
	}
	let { albumName }: Props = $props();
</script>

<h1>
	<a href="/albums" class="back-link">
		<svg><use href="#caret-left-icon" /></svg>
		back
	</a>
	Your {albumName} auto album:
</h1>
<div class="line" />

<style>
	h1 {
		display: flex;
		align-items: center;
		margin: 2rem 0 0;
		color: var(--text);
		font-size: 2rem;
		font-weight: 575;
	}

	.back-link {
		display: flex;
		justify-content: center;
		align-items: center;
		padding: 0.125rem 0.5rem;
		margin: 0 0.5rem;
		border-radius: 2rem;
		background-color: var(--muted);
		color: var(--muted-foreground);
		font-size: 1.125rem;
		font-weight: 500;
		text-decoration: none;
	}

	.back-link > svg {
		width: 1.25rem;
		height: 1.25rem;
		color: inherit;
		stroke-width: 2;
		transition: transform 0.15s ease-out;
	}

	.back-link:hover {
		background-color: var(--accent);
		color: var(--accent-foreground);
	}

	.back-link:hover > svg {
		transform: scale(1.08) translateX(-2px);
	}

	.line {
		width: 100%;
		height: 0.125rem;
		margin: 1rem 0;
		background-color: var(--secondary);
	}
</style>
