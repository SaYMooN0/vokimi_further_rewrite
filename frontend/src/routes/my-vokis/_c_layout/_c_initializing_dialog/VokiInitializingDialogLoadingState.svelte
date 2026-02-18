<script lang="ts">
	import CubesLoader from '$lib/components/loaders/CubesLoader.svelte';

	interface Props {
		text: string;
	}
	let { text }: Props = $props();
</script>

<div class="loading-container">
	<CubesLoader sizeRem={7} color="var(--primary)" />
	<p>{text}</p>
</div>

<style>
	.loading-container {
		display: flex;
		flex-direction: column;
		align-items: center;
		gap: 2rem;
		width: 100%;
		margin: auto 0;
	}

	.loading-container p {
		margin: 0;
		color: var(--secondary-foreground);
		font-size: 2.5rem;
		font-weight: 600;
		letter-spacing: 0.25px;
	}
</style>
