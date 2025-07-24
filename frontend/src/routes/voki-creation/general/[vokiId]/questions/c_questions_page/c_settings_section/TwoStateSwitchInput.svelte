<script lang="ts">
	let {
		value = $bindable(),
		trueLabel,
		trueIconId,
		falseLabel,
		falseIconId
	}: {
		value: boolean;
		trueIconId: string;
		trueLabel: string;
		falseLabel: string;
		falseIconId: string;
	} = $props<{
		value: boolean;
		trueLabel: string;
		trueIconId: string;
		falseLabel: string;
		falseIconId: string;
	}>();
</script>

<div class="switch-container">
	<div class="option" class:selected={value} onclick={() => (value = true)}>
		<svg><use href={trueIconId} /></svg>
		{trueLabel}
	</div>
	<div class="sep" />
	<div class="option" class:selected={!value} onclick={() => (value = false)}>
		<svg><use href={falseIconId} /></svg>
		{falseLabel}
	</div>
</div>

<style>
	.switch-container {
		display: grid;
		grid-template-columns: 1fr auto 1fr;
        width: fit-content;
		gap: 0.5rem;
		border-radius: 0.375rem;
	}
	.sep {
		width: 0.125rem;
		height: 100%;
		border-radius: 0.125rem;
		background-color: var(--muted);
	}
	.option {
		width: 10rem;
		padding: 0.25rem 0.5rem;
		border-radius: 0.5rem;
		gap: 0.25rem;
		background-color: var(--muted);
		color: var(--muted-foreground);
		cursor: pointer;
		display: grid;
		grid-template-columns: 1.5rem 1fr;
		align-items: center;
		font-size: 1.125rem;
		font-weight: 450;
		letter-spacing: 0.4px;
		box-shadow: var(--shadow-xs);
		transition: transform 0.12s ease-in;
	}
	.option > svg {
		width: 100%;
		aspect-ratio: 1/1;
		stroke-width: 1.8;
	}
	.option:not(.selected):hover {
		background-color: var(--accent);
		color: var(--accent-foreground);
		transform: scale(1.04);
	}
	.option.selected {
		background-color: var(--primary);
		color: var(--primary-foreground);
		box-shadow: var(--shadow-md);
	}
</style>
