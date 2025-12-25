<script lang="ts">
	import PrimaryButton from '$lib/components/buttons/PrimaryButton.svelte';

	interface Props {
		openPublishingConfirmationDialog: () => void;
	}
	let { openPublishingConfirmationDialog }: Props = $props();
</script>

<div class="ready-section">
	<div class="ready-content">
		<div class="ready-icon" aria-hidden="true">
			<svg><use href="#common-check-icon" /></svg>
		</div>

		<h2 class="ready-title">Voki is ready to be published</h2>
		<p class="ready-subtitle">This voki has no publishing issues and is ready to be published</p>

		<PrimaryButton onclick={() => openPublishingConfirmationDialog()}>
			Publish this voki
		</PrimaryButton>
	</div>
</div>

<style>
	.ready-section {
		position: relative;
		overflow: hidden;
		width: 100%;
		padding: 6rem 0;
		background: var(--back);
	}

	.ready-content {
		position: relative;
		display: flex;
		flex-direction: column;
		align-items: center;
		gap: 0.5rem;
		width: min(52rem, calc(100% - 2rem));
		margin: 0 auto;
		text-align: center;
	}

	.ready-icon {
		display: grid;
		place-items: center;
		width: 4.5rem;
		height: 4.5rem;
		border-radius: 999rem;
		background: var(--accent);
	}

	.ready-icon svg {
		width: 2.75rem;
		height: 2.75rem;
		color: var(--primary);
		stroke-width: 2;
	}

	.ready-title {
		font-size: 2.25rem;
		font-weight: 560;
		letter-spacing: 0.25px;
		text-wrap: balance;
	}

	.ready-subtitle {
		max-width: 46rem;
		color: var(--secondary-foreground);
		font-size: 1.25rem;
		text-wrap: balance;
	}

	.ready-content > :global(.primary-btn) {
		margin-top: 1rem;
	}
</style>
