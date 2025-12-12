<script lang="ts">
	import type { GeneralVokiResultsVisibility } from '$lib/ts/voki';
	import { watch } from 'runed';

	interface Props {
		value: GeneralVokiResultsVisibility;
		signedInOnlyTaking: boolean;
	}
	let { value = $bindable(), signedInOnlyTaking }: Props = $props();
	watch(()=>signedInOnlyTaking, ()=>{
		if(!signedInOnlyTaking){
			value = 'Anyone';
		}
	});
</script>

<div class="options">
	<div
		class="option unselectable"
		class:selected={value === 'Anyone'}
		onclick={() => (value = 'Anyone')}
	>
		Anyone
	</div>
	<div
		class="option unselectable"
		class:selected={value === 'AfterTaking'}
		class:disabled={!signedInOnlyTaking}
		onclick={() => (value = 'AfterTaking')}
	>
		After taking
	</div>
	<div
		class="option unselectable"
		class:selected={value === 'OnlyReceived'}
		class:disabled={!signedInOnlyTaking}

		onclick={() => (value = 'OnlyReceived')}
	>
		Only received
	</div>
</div>

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
	.option.disabled {
		background-color: var(--secondary);
		color: var(--secondary-foreground);
		opacity: 0.7;
		box-shadow: var(--shadow-xs);
		pointer-events: none;
	}
</style>
