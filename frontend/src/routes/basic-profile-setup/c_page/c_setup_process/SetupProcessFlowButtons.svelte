<script lang="ts">
	interface Props {
		prevButton: {
			appearance: () => 'primary' | 'secondary' | 'hidden';
			onClick: () => void;
		};
		nextButton: {
			appearance: () => 'primary' | 'secondary' | 'hidden';
			onClick: () => void;
		};
	}
	let { nextButton, prevButton }: Props = $props();
	let prevBtnAppearance = $derived(prevButton.appearance());
	let nextBtnAppearance = $derived(nextButton.appearance());
</script>

<div class="buttons-container">
	<button
		onclick={prevButton.onClick}
		class="btn prev-btn unselectable"
		class:primary={prevBtnAppearance === 'primary'}
		class:secondary={prevBtnAppearance === 'secondary'}
		class:hidden={prevBtnAppearance === 'hidden'}
		type="button">Previous step</button
	>

	<button
		onclick={nextButton.onClick}
		class="btn next-btn unselectable"
		class:primary={nextBtnAppearance === 'primary'}
		class:secondary={nextBtnAppearance === 'secondary'}
		class:hidden={nextBtnAppearance === 'hidden'}
		type="button">Next step</button
	>
</div>

<style>
	.buttons-container {
		display: flex;
		justify-content: space-between;
		align-items: center;
		gap: 1rem;
		padding-top: 0.75rem;
	}

	.btn {
		display: inline-flex;
		align-items: center;
		justify-content: center;
		font-weight: 550;
		font-size: 1.125rem;
		padding: 0.5rem 0;
		width: 12rem;
		border-radius: var(--radius);
		border: none;
		cursor: pointer;
	}

	.btn.primary {
		background-color: var(--primary);
		color: var(--primary-foreground);
	}

	.btn.primary:hover {
		background-color: var(--primary-hov);
	}

	.btn.secondary {
		background-color: var(--secondary);
		color: var(--secondary-foreground);
		box-shadow: var(--shadow-xs), var(--shadow-md);

	}

	.btn.secondary:hover {
		background-color: var(--muted);
		color: var(--muted-foreground);
	}

	.btn.hidden {
		visibility: hidden;
		pointer-events: none;
	}
</style>
