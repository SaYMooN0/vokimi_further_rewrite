<script lang="ts">
	interface Props {
		albumName: string;
		vokiCount: number;
	}
	let { albumName, vokiCount }: Props = $props();
</script>

<h1>
	<a href="/albums" class="back-link">
		<svg><use href="#caret-left-icon" /></svg>
		Back
	</a>
	Your {albumName} Vokis ({vokiCount})
</h1>
<div class="line" />

<style>
	h1 {
		margin: 2rem 0 0;
		color: var(--text);
		display: flex;
		align-items: center;
	}
	.back-link {
		margin: 0 0.5rem;
		background-color: var(--muted);
		color: var(--muted-foreground);
		padding: 0.25rem 0.5rem;
		border-radius: 0.25rem;
		text-decoration: none;
		display: flex;
		align-items: center;
		justify-content: center;
		gap: 0.25rem;
	}
	.back-link > svg {
		height: 1rem;
		width: 1rem;
		background-color: none;
		color: inherit;
	}
	.line {
		width: 100%;
		height: 0.125rem;
		background-color: var(--secondary);
		margin: 1rem 0;
	}
</style>
