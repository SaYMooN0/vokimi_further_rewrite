<script lang="ts">
	import LinesLoader from '$lib/components/loaders/LinesLoader.svelte';

	let { onclick, text, isLoading }: { onclick: () => void; text: string; isLoading: boolean } =
		$props<{
			onclick: () => void;
			text: string;
			isLoading: boolean;
		}>();
</script>

<button
	class="confirm-btn unselectable"
	onclick={() => onclick()}
	type="submit"
	disabled={isLoading}
	class:loading={isLoading}
>
	{#if isLoading}
		<LinesLoader sizeRem={1.3} strokePx={2} />
	{:else}
		{text}
	{/if}
</button>

<style>
	.confirm-btn {
		display: flex;
		flex-direction: row;
		justify-content: center;
		align-items: center;
		width: 92%;
		min-height: 2.25rem;
		margin: 1rem auto 0;
		border: none;
		border-radius: 0.25rem;
		background-color: var(--primary);
		color: var(--primary-foreground);
		font-size: 1.5rem;
		font-weight: 450;
		letter-spacing: 0.5px;
		box-shadow: var(--shadow);
		transition: all 0.12s ease-in;
		cursor: pointer;
	}

	.confirm-btn:hover {
		background-color: var(--primary-hov);
	}

	.confirm-btn:active {
		width: 90%;
	}

	.confirm-btn.loading {
		pointer-events: none;
		opacity: 0.9;
	}

	.confirm-btn > :global(.container) {
		--loader-color: var(--primary-foreground);
	}
</style>
