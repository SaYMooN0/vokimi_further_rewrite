<script lang="ts">
	import type { Snippet } from 'svelte';

	let { vokiId, children }: { vokiId: string; children: Snippet } = $props();
</script>

<div class="session-err-container">
	<svg class="err-icon"><use href="#common-error-icon"></use></svg>
	<div class="message">
		{@render children()}
	</div>
	<a href={`/catalog/${vokiId}`} class="back-link">Go to Voki catalog page</a>
</div>

<style>
	.session-err-container {
		display: flex;
		justify-content: center;
		flex-direction: column;
		align-items: center;
		margin: 2rem auto;
		min-height: 60vh;
		padding: 1rem;
		max-width: 40rem;
		gap: 1rem;
	}

	.err-icon {
		background-color: var(--accent);
		color: var(--accent-foreground);
		width: 4rem;
		height: 4rem;
		padding: 0.675rem;
		border-radius: 30%;
		stroke-width: 1.5;
	}
	.message {
		color: var(--text);
		font-size: 1.25rem;
		line-height: 1.25;
		font-weight: 400;
		text-align: center;
		font-weight: 425;
	}

	.back-link {
		display: inline-flex;
		align-items: center;
		justify-content: center;
		padding: 0.75rem 1.5rem;
		border-radius: 0.5rem;
		background-color: var(--primary);
		color: var(--primary-foreground);
		font-size: 1rem;
		font-weight: 500;
		text-decoration: none;
		transition: all 0.2s ease;
	}

	.back-link:hover {
		background-color: var(--primary-hov);
	}
</style>
