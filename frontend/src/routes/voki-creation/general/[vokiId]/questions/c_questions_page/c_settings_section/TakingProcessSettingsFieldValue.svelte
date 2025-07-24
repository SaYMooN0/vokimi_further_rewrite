<script lang="ts">
	const { text, iconId }: { text: string; iconId: string } = $props<{
		text: string;
		iconId: string;
	}>();
</script>

<div class="field">
	<svg><use href={iconId} /></svg>

	<label>{text}</label>
</div>

<style>
	.field {
		width: 10rem;
		padding: 0.25rem 0.5rem;
		border-radius: 0.5rem;
		gap: 0.375rem;
		background-color: var(--muted);
		color: var(--muted-foreground);
		display: grid;
		grid-template-columns: 1.5rem 1fr;
		align-items: center;
		font-size: 1.125rem;
		font-weight: 450;
		letter-spacing: 0.4px;
	}
	.field > svg {
		width: 100%;
		aspect-ratio: 1/1;
		stroke-width: 1.8;
	}
</style>
