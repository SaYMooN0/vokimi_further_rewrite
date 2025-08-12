<script lang="ts">
	import PrimaryButton from '$lib/components/PrimaryButton.svelte';

	const vokiCreationApi = getVokiCreationPageApiService();
	const { vokiId }: { vokiId: string } = $props<{ vokiId: string }>();
</script>

<div class="no-issues-container">
	<label class="no-issues-label"
		>This voki has no publishing issues and is ready to be published</label
	>
	<PrimaryButton onclick={() => vokiCreationApi.publishVoki(vokiId)}
		>Publish this voki</PrimaryButton
	>
</div>

<style>
	.no-issues-container {
		display: flex;
		flex-direction: column;
		align-items: center;
		width: 52rem;
		height: 18rem;
		padding: 2rem;
		margin: 0 auto;
		margin-top: 8rem;
		border-radius: 1rem;
		box-shadow: var(--shadow-2xl);
		animation: var(--default-fade-in-animation);
	}
</style>
