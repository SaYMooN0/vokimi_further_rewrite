<script lang="ts">
	const { onClick, text } = $props<{ onClick: () => void; text: string }>();
</script>

<p class="unselectable" onclick={() => onClick()}>{text}</p>

<style>
	p {
		width: fit-content;
		padding: 0.125rem 0.75rem;
		margin-top: 0.25rem;
		border-radius: 0.375rem;
		background-color: transparent;
		color: var(--gray);
		font-size: 1rem;
		transition: all 0.12s ease;
		background-color: var(--muted);
		color: var(--muted-foreground);
        text-decoration: underline;
		cursor: pointer;
	}

	p:hover {
		color: var(--primary);
	}
	p:active {
		transform: scale(0.98);
		color: var(--primary-hov);
	}
</style>
