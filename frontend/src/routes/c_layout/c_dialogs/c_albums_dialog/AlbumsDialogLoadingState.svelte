<script>
	import CubesLoader from '$lib/components/loaders/CubesLoader.svelte';
</script>

<div class="loading-container">
	<CubesLoader sizeRem={6} />
	<h1>Loading your albums</h1>
</div>

<style>
	.loading-container {
		display: flex;
		flex-direction: column;
		justify-content: center;
		align-items: center;
		gap: 2rem;
		width: 100%;
		height: 100%;
		padding-bottom: 2rem;
	}

	.loading-container > h1 {
		color: var(--secondary-foreground);
		font-size: 2rem;
		font-weight: 550;
		letter-spacing: 0.5px;
	}
</style>
