<script lang="ts">
	import PrimaryButton from '$lib/components/PrimaryButton.svelte';

	const { addNewAnswer }: { addNewAnswer: () => void } = $props<{ addNewAnswer: () => void }>();
</script>

<div class="container">
	<h1>This question has no answers yet</h1>
	<PrimaryButton onclick={addNewAnswer}>Add first answer</PrimaryButton>
</div>

<style>
	.container {
		display: flex;
		flex-direction: column;
		align-items: center;
		gap: 1.5rem;
		margin-top: 2rem;
		background-color: var(--secondary);
		border-radius: 1rem;
		padding: 2.5rem 4rem;
		width: fit-content;
		align-self: center;
		border: 0.125rem var(--secondary-foreground) dashed;
	}
	h1{
		font-size: 1.875rem;
		color: var(--secondary-foreground);
		font-weight: 550;
	}
</style>
