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
		margin: 8rem auto;
        width: fit-content;
		display: flex;
		flex-direction: column;
		align-items: center;
		gap: 1rem;
		background-color: var(--secondary);
		border: var(--primary) 0.25rem dashed;
        padding: 3rem 6rem;
        border-radius: 1rem;
	}
	.message-container h1 {
		color: var(--secondary-foreground);
		text-align: center;
        font-size: 2rem;
        font-weight: 540;
	}
</style>
