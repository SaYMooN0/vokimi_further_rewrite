<script lang="ts">
	import type { Snippet } from 'svelte';

	let { vokiId, children }: { vokiId: string; children: Snippet } = $props();
</script>

<div class="session-err-container">
	<div class="card">
		<div class="icon-wrapper">
			<svg class="err-icon"><use href="#common-error-icon"></use></svg>
		</div>
		<div class="message">
			{@render children()}
		</div>
		<a href={`/catalog/${vokiId}`} class="back-link"> See the Voki in catalog </a>
	</div>
</div>

<style>
	.session-err-container {
		display: flex;
		justify-content: center;
		align-items: center;
		min-height: 60vh;
		padding: 1rem;
	}

	.card {
		display: flex;
		flex-direction: column;
		align-items: center;
		gap: 2rem;
		max-width: 32rem;
		padding: 3rem;
		background-color: white;
		border-radius: 1rem;
		box-shadow: var(--shadow-xl);
		text-align: center;
		animation: fade-in 0.4s ease-out;
	}

	.icon-wrapper {
		display: flex;
		align-items: center;
		justify-content: center;
		width: 5rem;
		height: 5rem;
		border-radius: 50%;
		background-color: var(--red-1);
		color: var(--red-5);
	}

	.message {
		color: var(--text);
		font-size: 1.25rem;
		line-height: 1.6;
		font-weight: 400;
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
		box-shadow: var(--shadow-md);
	}

	.back-link:hover {
		background-color: var(--primary-hov);
		transform: translateY(-1px);
		box-shadow: var(--shadow-lg);
	}

	.back-link:active {
		transform: translateY(0);
		box-shadow: var(--shadow-sm);
	}

	@keyframes fade-in {
		from {
			opacity: 0;
			transform: translateY(10px);
		}
		to {
			opacity: 1;
			transform: translateY(0);
		}
	}
</style>
