<script lang="ts">
	let { vokiName }: { vokiName: string } = $props<{ vokiName: string }>();
</script>

<span class="voki-name">{vokiName}</span>

<style>
	.voki-name {
		max-width: 18rem;
		margin: 0 0.25rem;
		color: var(--primary);
		font-size: inherit;
		font-weight: 640;
		text-overflow: ellipsis;
		overflow: hidden;
		white-space: nowrap;
	}
</style>
