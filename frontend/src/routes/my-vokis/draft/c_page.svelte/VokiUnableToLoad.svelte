<script lang="ts">
	const { vokiId }: { vokiId: string } = $props<{ vokiId: string }>();
</script>

<div class="unable-to-load">
	<h1>Unable to load voki data</h1>
	<label>Voki id: {vokiId}</label>
</div>

<style>
	.unable-to-load {
		display: flex;
		flex-direction: column;
		justify-content: center;
		align-items: center;
		padding: 2rem 1rem;
		border-radius: 0.5rem;
		background-color: var(--err-back);
		color: var(--err-foreground);
		place-self: center center;
	}

	.unable-to-load h1 {
		font-size: 1.25rem;
		font-weight: 450;
	}

	.unable-to-load label {
		margin-top: 0.5rem;
		font-size: 1rem;
		font-weight: 450;
	}
</style>
