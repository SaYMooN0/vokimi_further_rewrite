<script lang="ts">
	import UserRatingStarsInput from './UserRatingStarsInput.svelte';
	interface Props {
		saveNewUserRating: (value: number) => void;
	}
	let { saveNewUserRating }: Props = $props();
	let inputValue = $state(0);
</script>

<div class="first-time-voki">
	Rate this Voki:<UserRatingStarsInput bind:starsValue={inputValue} />
	{#if inputValue != 0}
		<svg class="btn close-btn" type="button" onclick={() => (inputValue = 0)}
			><use href="#common-cross-icon" /></svg
		>
		<button class="btn save-btn" onclick={() => saveNewUserRating(inputValue)}>Save</button>
	{/if}
</div>

<style>
	.first-time-voki {
		display: flex;
		align-items: center;
		flex-direction: row;
		font-size: 1.25rem;
		font-weight: 450;
		cursor: default;
		gap: 0.75rem;
	}
	.btn {
		height: 1.675rem;
		width: fit-content;
		border-radius: 0.25rem;
        cursor: pointer;
	}
	.close-btn {
		aspect-ratio: 1/1;
		background-color: var(--muted);
		padding: 0.125rem;
		color: var(--muted-foreground);
		stroke-width: 2.2;
		transition: all 0.04s ease-in;
	}
	.close-btn:hover {
		background-color: var(--err-foreground);
		color: var(--err-back);
	}

	.save-btn {
		background-color: var(--primary);
		color: var(--primary-foreground);
		border: none;
		padding: 0 1rem;
		font-weight: 450;
		font-size: 1.125rem;
	}
	.save-btn:hover {
		background-color: var(--primary-hov);
	}
</style>
