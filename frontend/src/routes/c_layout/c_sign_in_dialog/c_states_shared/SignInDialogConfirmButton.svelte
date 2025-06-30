<script lang="ts">
	const { onclick, text, isLoading }: { onclick: () => void; text: string; isLoading: boolean } =
		$props<{
			onclick: () => void;
			text: string;
			isLoading: boolean;
		}>();
</script>

<button class="confirm-btn unselectable" onclick={() => onclick()} type="submit" disabled={isLoading}>
	{#if isLoading}
		Loading...
	{:else}
		{text}
	{/if}
</button>

<style>
	.confirm-btn {
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
</style>
