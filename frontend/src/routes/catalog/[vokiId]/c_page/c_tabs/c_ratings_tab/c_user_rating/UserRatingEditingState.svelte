<script lang="ts">
	import VokiPageTabSectionLabel from "../../c_tabs_shared/VokiPageTabSectionLabel.svelte";

	interface Props {
		starsValue: number;
		onSaveButtonClicked: () => void;
		onCancelButtonClick: () => void;
	}
	let { starsValue = $bindable(), onSaveButtonClicked, onCancelButtonClick }: Props = $props();

	const stars = Array.from({ length: 5 }, (_, i) => i + 1);
	let starHovered: number | null = $state(null);
</script>

<div class="rating-input">
	<VokiPageTabSectionLabel fieldName="Your rating:" />
	<div class="stars-input">
		{#each stars as i}
			<button
				type="button"
				class="star-btn"
				aria-label={`Set rating ${i} of ${5}`}
				onmouseenter={() => (starHovered = i)}
				onmouseleave={() => (starHovered = null)}
				onclick={() => (starsValue = i)}
			>
				<svg
					class="star"
					class:filled={starsValue >= i}
					class:highlight={starHovered !== null && starHovered >= i}
					viewBox="0 0 24 24"
				>
					<use href="#common-star-icon" />
				</svg>
			</button>
		{/each}
	</div>
	{#if starsValue != 0}
		<svg class="btn close-btn" type="button" onclick={() => onCancelButtonClick()}
			><use href="#common-cross-icon" /></svg
		>
		<button class="btn save-btn" onclick={() => onSaveButtonClicked()}>Save</button>
	{/if}
</div>

<style>
	.rating-input {
		display: flex;
		flex-direction: row;
		align-items: center;
		gap: 0.75rem;
		font-size: 1.25rem;
		font-weight: 450;
		cursor: default;
	}
	.stars-input {
		display: flex;
		flex-direction: row;
		gap: 0;
	}

	.star {
		width: 1.75rem;
		height: 1.75rem;
		padding: 0.125rem;
		color: var(--secondary-foreground);
		transition: transform 0.06s ease;
		fill: none;
	}

	.star:not(.filled) {
		stroke-width: 2;
	}

	.star:hover {
		transform: scale(1.1);
	}

	.highlight {
		color: var(--primary);
	}

	.filled {
		color: var(--primary);
		fill: var(--primary);
	}

	.star-btn {
		padding: 0;
		border: none;
		background: transparent;
		cursor: pointer;
		appearance: none;
	}
	.btn {
		width: fit-content;
		height: 1.675rem;
		border-radius: 0.25rem;
		cursor: pointer;
	}

	.close-btn {
		padding: 0.125rem;
		background-color: var(--muted);
		color: var(--muted-foreground);
		transition: all 0.04s ease-in;
		aspect-ratio: 1/1;
		stroke-width: 2.2;
	}

	.close-btn:hover {
		background-color: var(--err-foreground);
		color: var(--err-back);
	}

	.save-btn {
		padding: 0 1rem;
		border: none;
		background-color: var(--primary);
		color: var(--primary-foreground);
		font-size: 1.125rem;
		font-weight: 450;
	}

	.save-btn:hover {
		background-color: var(--primary-hov);
	}
</style>
