<script lang="ts">
	let { color = $bindable() }: { color: string } = $props<{
		color: string;
	}>();
</script>

<div class="inputs">
	<div class="color-input-wrap">
		<input type="color" bind:value={color} class="color-input" />
	</div>
	<input type="text" bind:value={color} class="hex-input" />
</div>

<style>
	.inputs {
		display: flex;
		flex-direction: column;
		gap: 0.5rem;
		align-items: center;
		width: 8rem;
	}

	.color-input-wrap {
		width: 100%;
		position: relative;
		display: inline-block;
		height: 6rem;
		--color-input-radius: 0.5rem;
	}

	.color-input {
		-webkit-appearance: none;
		appearance: none;
		border: none;
		padding: 0;
		width: 100%;
		height: 100%;
		border-radius: var(--color-input-radius);
		cursor: pointer;
	}

	.color-input::-webkit-color-swatch-wrapper {
		padding: 0;
		border-radius: var(--color-input-radius);
	}
	.color-input::-webkit-color-swatch {
		border: none;
		border-radius: var(--color-input-radius);
	}

	.color-input::-moz-color-swatch {
		border: none;
		border-radius: var(--color-input-radius);
	}

	.color-input-wrap::after {
		content: '';
		position: absolute;
		inset: 0;
		border-radius: var(--color-input-radius);
		border: 0.125rem solid var(--secondary-foreground);
		box-shadow: var(--shadow-xs);
		pointer-events: none;
	}

	.hex-input {
		width: 100%;
		font-size: 1.25rem;
		padding: 0.125rem 0.6rem;
		border-radius: 0.5rem;
		color: var(--secondary-foreground);
		background-color: var(--secondary);
		outline: none;
		border: 0.125rem solid var(--secondary-foreground);
	}
	.hex-input:hover,
	.hex-input:focus {
		border-color: var(--primary);
	}
</style>
