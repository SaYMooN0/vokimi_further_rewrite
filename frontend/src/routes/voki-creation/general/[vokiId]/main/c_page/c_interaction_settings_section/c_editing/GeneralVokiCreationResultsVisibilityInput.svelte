<script lang="ts">
	import type { GeneralVokiResultsVisibility } from '$lib/ts/voki';

	interface Props {
		value: GeneralVokiResultsVisibility;
	}
	let { value = $bindable() }: Props = $props();
</script>

<div class="options">
	{@render option('Anyone', 'Anyone')}
	{@render option('AfterTaking', 'After taking')}
	{@render option('OnlyReceived', 'Only received')}
</div>

{#snippet option(optionVal: GeneralVokiResultsVisibility, optionValName: string)}
	<div
		class="option unselectable"
		class:selected={value === optionVal}
		onclick={() => (value = optionVal)}
	>
		{optionValName}
	</div>
{/snippet}

<style>
	.options {
		display: flex;
		flex-direction: row;
		gap: 1rem;
		margin-left: 1rem;
	}

	.option {
		background-color: var(--muted);
		color: var(--muted-foreground);
		cursor: default;
		width: 9rem;
		display: flex;
		align-items: center;
		justify-content: center;
		padding: 0.25rem;
		border-radius: 0.5rem;
		font-size: 1.125rem;
		font-weight: 450;
		transition: all 0.04s ease-in;
	}
	.option:hover {
		background-color: var(--accent);
		color: var(--accent-foreground);
	}
	.option:active {
		transform: scale(0.98);
	}
	.option.selected {
		background-color: var(--primary);
		color: var(--primary-foreground);
	}
</style>
