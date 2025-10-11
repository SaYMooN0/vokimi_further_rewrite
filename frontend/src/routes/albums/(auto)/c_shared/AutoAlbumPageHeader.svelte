<script lang="ts">
	interface Props {
		albumName: string;
	}
	let { albumName }: Props = $props();
</script>

<h1>
	<a href="/albums" class="back-link">
		<svg><use href="#caret-left-icon" /></svg>
		Back
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
	}

	.back-link {
		display: flex;
		justify-content: center;
		align-items: center;
		gap: 0.25rem;
		padding: 0.25rem 0.5rem;
		margin: 0 0.5rem;
		border-radius: 0.25rem;
		background-color: var(--muted);
		color: var(--muted-foreground);
		text-decoration: none;
	}

	.back-link > svg {
		width: 1rem;
		height: 1rem;
		background-color: none;
		color: inherit;
	}

	.line {
		width: 100%;
		height: 0.125rem;
		margin: 1rem 0;
		background-color: var(--secondary);
	}
</style>
