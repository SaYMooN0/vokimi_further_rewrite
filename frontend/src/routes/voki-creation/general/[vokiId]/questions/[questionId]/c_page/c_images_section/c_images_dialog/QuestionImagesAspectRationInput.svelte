<script lang="ts">
	import NumberSliderInput from '$lib/components/inputs/NumberSliderInput.svelte';

	let { aspectRatio = $bindable() }: { aspectRatio: { width: number; height: number } } = $props<{
		aspectRatio: {
			width: number;
			height: number;
		};
	}>();
	let defaultOptions = [
		{ width: 16, height: 9 },
		{ width: 4, height: 3 },
		{ width: 1, height: 1 },
		{ width: 3, height: 4 },
		{ width: 9, height: 16 }
	];
	let isCustom = $state<boolean>(
		!defaultOptions.some((o) => o.width === aspectRatio.width && o.height === aspectRatio.height)
	);
	function selectDefault(option: { width: number; height: number }) {
		isCustom = false;
		aspectRatio = option;
	}
	function onCustomClick() {
		if (isCustom) return;

		isCustom = true;

		const { width, height } = aspectRatio;

		if (width >= height) {
			const newWidth = Number((width / height).toFixed(2));
			aspectRatio = { width: newWidth, height: 1 };
		} else {
			const newHeight = Number((height / width).toFixed(2));
			aspectRatio = { width: 1, height: newHeight };
		}
	}
</script>

<div class="aspect-ratio-inputs-container">
	<div class="main-options unselectable">
		{#each defaultOptions as option}
			<button
				class="aspect-ratio-option"
				class:chosen={option.width === aspectRatio.width && option.height === aspectRatio.height}
				onclick={() => selectDefault(option)}
			>
				{option.width} : {option.height}
			</button>
		{/each}
		<button
			class="aspect-ratio-option custom-btn"
			class:chosen={isCustom}
			onclick={() => onCustomClick()}>Custom</button
		>
	</div>
	<div class="custom-inputs" class:open={isCustom}>
		<NumberSliderInput
			bind:value={aspectRatio.width}
			min={1}
			max={3}
			step={0.01}
			marks={[1, 2, 3]}
		/>
		<label>
			{aspectRatio.width} : {aspectRatio.height}
		</label>
		<NumberSliderInput
			bind:value={aspectRatio.height}
			min={1}
			max={3}
			step={0.01}
			marks={[1, 2, 3]}
		/>
	</div>
</div>

<style>
	.aspect-ratio-inputs-container {
		display: flex;
		flex-direction: column;
		gap: 1rem;
	}
	.main-options {
		display: flex;
		flex-direction: row;
		align-items: center;
		justify-content: center;
		width: fit-content;
		gap: 1rem;
	}
	.aspect-ratio-option {
		background-color: var(--muted);
		color: var(--muted-foreground);
		border-radius: 2rem;
		box-shadow: var(--shadow);
		font-weight: 450;
		font-size: 1.25rem;
		height: 2rem;
		width: 5rem;
		border: none;
	}
	.aspect-ratio-option:hover {
		background-color: var(--accent);
		color: var(--accent-foreground);
	}
	.aspect-ratio-option.chosen {
		font-weight: 500;
		background-color: var(--primary);
		color: var(--primary-foreground);
	}
	.custom-btn {
		width: 8rem;
		margin-left: 1rem;
	}
	.custom-inputs {
		opacity: 0;
		height: 0;
		transition: all 0.2s ease-in;
		interpolate-size: allow-keywords;
		font-size: 0;
		display: grid;
		grid-template-columns: 1fr 8rem 1fr;
		align-items: center;
		gap: 1rem;
	}

	.custom-inputs:not(.open) > :global(*) {
		display: none;
	}
	.custom-inputs.open {
		height: auto;
		opacity: 1;
		font-size: 1.25rem;
	}
	.custom-inputs label {
		font-size: 1.25rem;
		color: var(--muted-foreground);
		border-radius: 2rem;
		font-weight: 450;
		height: 2rem;
		align-items: center;
		justify-content: center;
		display: flex;
		flex-direction: row;
		gap: 0.375rem;
		width: 100%;
		margin-bottom: 1rem;
	}
</style>
