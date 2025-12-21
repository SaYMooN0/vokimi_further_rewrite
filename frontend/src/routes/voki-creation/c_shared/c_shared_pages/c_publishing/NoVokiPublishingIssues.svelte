<script lang="ts">
	import PrimaryButton from '$lib/components/buttons/PrimaryButton.svelte';

	interface Props {
		openPublishingConfirmationDialog: () => void;
	}
	let { openPublishingConfirmationDialog }: Props = $props();
</script>

<div class="no-issues-container">
	<label class="no-issues-label"
		>This voki has no publishing issues and is ready to be published</label
	>
	<PrimaryButton onclick={() => openPublishingConfirmationDialog()}>Publish this voki</PrimaryButton
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
		animation: var(--default-fade-in-animation);
	}

	.no-issues-label {
		margin-bottom: auto;
		font-size: 2rem;
		font-weight: 520;
		text-align: center;
		letter-spacing: 0.25px;
		text-wrap: balance;
	}

	.no-issues-container > :global(.primary-btn) {
		margin-top: 1rem;
	}
</style>
