<script lang="ts">
	import PrimaryButton from '$lib/components/PrimaryButton.svelte';

	const { openQuestionInitializingDialog }: { openQuestionInitializingDialog: () => void } =
		$props<{ openQuestionInitializingDialog: () => void }>();
</script>

<div class="message-container">
	<h1>This voki doesn't have any questions yet</h1>
	<PrimaryButton onclick={openQuestionInitializingDialog}>Create first question</PrimaryButton>
</div>

<style>
	.message-container {
		display: flex;
		flex-direction: column;
		align-items: center;
		gap: 1rem;
        width: fit-content;
        padding: 3rem 6rem;
		margin: 8rem auto;
		border: var(--primary) 0.25rem dashed;
        border-radius: 1rem;
		background-color: var(--secondary);
	}

	.message-container h1 {
		color: var(--secondary-foreground);
        font-size: 2rem;
        font-weight: 540;
		text-align: center;
	}
</style>
