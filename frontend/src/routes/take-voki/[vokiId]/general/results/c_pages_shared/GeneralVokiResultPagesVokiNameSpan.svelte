<script lang="ts">
	let { vokiName }: { vokiName: string } = $props<{ vokiName: string }>();
</script>

<span class="voki-name">{vokiName}</span>

<style>
	.voki-name {
		display: inline-block;
		max-width: calc(40vw - 4rem);
		margin: 0 0.25rem;
		color: var(--accent);
		font-size: inherit;
		font-weight: 650;
		text-overflow: ellipsis;
		overflow: hidden;
		white-space: nowrap;
	}
</style>
