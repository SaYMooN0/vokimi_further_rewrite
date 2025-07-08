<script lang="ts">
	let { vokiName, typeName }: { vokiName: string; typeName: string } = $props<{
		vokiName: string;
		typeName: string;
	}>();
</script>

<h1>Creation of the new <label>{vokiName}</label>{typeName} voki</h1>

<style>
	h1 {
		display: flex;
		align-items: center;
		justify-content: center;
		color: var(--secondary-foreground);
		font-size: 1.25rem;
		height: var(--side-bar-links-top-padding);
        font-weight: 440;
	}

	h1 label {
		display: inline-block;
		max-width: calc(60vw - 4rem);
        margin: 0 0.25rem;
		text-overflow: ellipsis;
		overflow: hidden;
		white-space: nowrap;
		color: var(--primary);
        font-weight: 490;

	}
</style>
