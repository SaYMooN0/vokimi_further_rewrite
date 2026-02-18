<script lang="ts">
	interface Props {
		vokiId: string;
	}
	let { vokiId }: Props = $props();
</script>

<div class="exists-failed-container">
	<div class="icon-wrapper">
		<svg class="icon">
			<use href="#common-info-icon" />
		</svg>
	</div>
	<h3>Voki Initialized with Delay</h3>
	<p>
		Your voki has been initialized, but it might take a few more seconds to appear in the creation
		workspace.
	</p>
	<p>
		You can access it from your <a href="/my-vokis/draft-vokis?vokiHighlight={vokiId}">Drafts</a>.
	</p>
</div>

<style>
	.exists-failed-container {
		display: flex;
		flex-direction: column;
		align-items: center;
		gap: 1rem;
		width: 100%;
		margin: auto 0;
		text-align: center;
	}

	.exists-failed-container .icon-wrapper {
		display: flex;
		align-items: center;
		justify-content: center;
		width: 5rem;
		height: 5rem;
		border-radius: 50%;
		background-color: var(--warn-back);
		color: var(--warn-foreground);
	}

	.exists-failed-container .icon {
		width: 2.5rem;
		height: 2.5rem;
		stroke-width: 2;
	}

	.exists-failed-container h3 {
		margin-bottom: 0.5rem;
		font-size: 1.75rem;
		font-weight: 600;
		color: var(--text);
	}

	.exists-failed-container p {
		max-width: 40ch;
		color: var(--secondary-foreground);
		font-size: 1.125rem;
		line-height: 1.5;
	}

	.exists-failed-container p a {
		color: var(--primary);
		text-decoration: underline;
		font-weight: 500;
	}
</style>
