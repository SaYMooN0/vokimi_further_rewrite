<div class="skeleton-item">
	<div class="skeleton-cover shimmer"></div>
	<div class="skeleton-name shimmer"></div>
</div>

<style>
	.skeleton-item {
		width: 100%;
		display: flex;
		flex-direction: column;
		gap: var(--voki-cover-name-gap);
	}

	.skeleton-cover,
	.skeleton-name {
		width: 100%;
		background-color: var(--secondary);
		border-radius: var(--voki-cover-border-radius);
		position: relative;
		overflow: hidden;
		color: var(--primary);
	}

	.skeleton-cover {
		aspect-ratio: var(--voki-cover-aspect-ratio);
	}

	.skeleton-name {
		height: var(--voki-name-max-height);
	}

	.shimmer::before {
		content: '';
		position: absolute;
		inset: 0;
		transform: translateX(-90%);
		background: linear-gradient(
			to right,
			transparent 0%,
			color-mix(in srgb, var(--secondary-foreground) 10%, var(--secondary) 10%) 50%,
			transparent 100%
		);
		animation: shimmer 2s infinite;
	}

	@keyframes shimmer {
		100% {
			transform: translateX(90%);
		}
	}
</style>
