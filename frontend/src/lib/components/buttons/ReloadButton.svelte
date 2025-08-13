<script lang="ts">
	interface Props {
		showIcon?: boolean;
		text?: string;
		onclick: () => void;
		className?: string;
	}
	let { showIcon = true, text = 'Reload', onclick, className = '' }: Props = $props();
</script>

<button class={`reload-btn ${className}`} onclick={() => onclick()}>
	{#if showIcon}
		<svg><use href="#common-reload-icon" /></svg>
	{/if}
	{text}
</button>

<style>
	.reload-btn {
		width: fit-content;
		padding: 0.25rem 1rem;
		margin: 0 auto;
		margin-bottom: 2rem;
		border-radius: 1rem;
		background-color: var(--secondary);
		color: var(--secondary-foreground);
		font-size: 1.125rem;
		font-weight: 440;
		cursor: pointer;
		display: flex;
		align-items: center;
		gap: 0.25rem;
		border: none;
		transition: transform 0.08s ease-out;
	}

	.reload-btn > svg {
		height: 1.125rem;
		width: 1.125rem;
		transition: transform 0.2s ease-out;
		stroke-width: 2.23;
	}
	.reload-btn:hover > svg {
		transform: rotate(60deg);
	}
	.reload-btn:active {
		transform: scale(0.96);
	}
	.reload-btn:active > svg {
		transform: rotate(180deg);
	}
</style>
