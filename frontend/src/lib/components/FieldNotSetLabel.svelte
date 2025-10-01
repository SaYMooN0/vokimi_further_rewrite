<script lang="ts">
	const { text, className = '' }: { text: string; className?: string } = $props<{
		text: string;
		className?: string;
	}>();
</script>

<label class="not-set {className}">
	<svg>
		<use href="#common-crossed-circle-icon" />
	</svg>{text}
</label>

<style>
	.not-set {
		text-indent: 0;
		display: flex;
		flex-direction: row;
		align-items: center;
		gap: 0.375rem;
		width: fit-content;
		padding: 0.25rem 0.75rem;
		margin-left: 0.5rem;
		border-radius: 0.5rem;
		background-color: var(--secondary);
		color: var(--secondary-foreground);
		font-size: 1rem;
		font-weight: 440;
		box-shadow: var(--shadow-xs);
	}

	.not-set > svg {
		height: 1.125rem;
		aspect-ratio: 1/1;
		stroke-width: 1.8;
	}
</style>
