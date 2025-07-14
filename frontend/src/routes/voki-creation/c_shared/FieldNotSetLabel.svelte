<script lang="ts">
	const { text, className="" }: { text: string; className?: string } = $props<{
		text: string;
		className?: string;
	}>();
</script>

<label class="not-set {className}">
	<svg>
		<use href="#crossed-circle-icon" />
	</svg>
	{text}
</label>

<style>
	.not-set {
		margin-left: 0.5rem;
		background-color: var(--secondary);
		color: var(--secondary-foreground);
		padding: 0.25rem 0.75rem;
		width: fit-content;
		display: flex;
		flex-direction: row;
		align-items: center;
		border-radius: 0.5rem;
		gap: 0.25rem;
		font-weight: 440;
	}
	.not-set > svg {
		height: 1.25rem;
		aspect-ratio: 1/1;
		stroke-width: 1.8;
	}
</style>
