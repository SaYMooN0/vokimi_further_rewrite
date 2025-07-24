<script lang="ts">
	let {
		label,
		isSelected,
		onClick
	}: {
		label: string;
		isSelected: boolean;
		onClick: () => void;
	} = $props<{
		label: string;
		isSelected: boolean;
		onClick: () => void;
	}>();
</script>

<div class="type-card" class:selected={isSelected} onclick={onClick}>
	{label}
</div>

<style>
	.type-card {
		display: flex;
		flex-direction: column;
		justify-content: center;
		align-items: center;
		gap: 0.25rem;
		width: 11.5rem;
		padding: 0.75rem 0;
		border-radius: 0.5rem;
		background-color: var(--secondary);
		color: var(--text);
		font-size: 1.25rem;
		font-weight: bold;
		font-weight: 580;
		text-align: center;
		letter-spacing: 0.5px;
		transition:
			transform 0.1s ease-in;
		cursor: pointer;
		white-space: nowrap;

	}

	.type-card:hover {
		transform: scale(1.02);
	}

	.type-card.selected {
		background-color: var(--primary);
		color: var(--primary-foreground);
		box-shadow: var(--shadow-xl);
		transition: transform 0.18s ease-in;
		transform: scale(1.06);
	}
</style>
