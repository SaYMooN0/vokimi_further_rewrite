<script lang="ts">
	interface Props {
		starsValue?: number;
	}
	let { starsValue = $bindable(0) }: Props = $props();

	const stars = Array.from({ length: 5 }, (_, i) => i + 1);

	let hovered: number | null = $state(null);

	function setVal(v: number) {
		starsValue = v;
	}
</script>

<div class="stars-input">
	{#each stars as i}
		<button
			type="button"
			class="star-btn"
			aria-label={`Set rating ${i} of ${5}`}
			onmouseenter={() => (hovered = i)}
			onmouseleave={() => (hovered = null)}
			onclick={() => setVal(i)}
		>
			<svg
				class="star"
				class:filled={starsValue >= i}
				class:highlight={hovered !== null && hovered >= i}
				viewBox="0 0 24 24"
			>
				<use href="#common-star-icon" />
			</svg>
		</button>
	{/each}
</div>

<style>
	.stars-input {
		display: flex;
	}
	.star {
		width: 1.75rem;
		height: 1.75rem;
		color: var(--secondary-foreground);
		fill: none;
		padding: 0.125rem;
		transition: transform 0.06s ease;
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
		appearance: none;
		background: transparent;
		border: none;
		padding: 0;
		cursor: pointer;
	}
</style>
