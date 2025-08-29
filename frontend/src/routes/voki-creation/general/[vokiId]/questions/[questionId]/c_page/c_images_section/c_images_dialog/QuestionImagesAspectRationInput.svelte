<script lang="ts">
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
		if (isCustom) {
			return;
		}
		isCustom = true;
		const newWidth = Number((aspectRatio.width / aspectRatio.height).toFixed(3));
		const newHeight = 1;

		aspectRatio = { width: newWidth, height: newHeight };
	}
</script>

<div class="options-container">
	{#each defaultOptions as option}
		<button class="aspect-ratio-option" onclick={() => selectDefault(option)}>
			{option.width} : {option.height}
		</button>
	{/each}
	<h1>{isCustom}</h1>
	<button onclick={() => onCustomClick()}>Custom</button>
	{#if isCustom}
		<div>{aspectRatio.width}:{aspectRatio.height}</div>
	{/if}
</div>
