<script lang="ts">
	interface Props {
		publicationDate: Date;
		content: { isLoading: true } | { isLoading: false; snapshots: VokiDailyRatingsSnapshot[] };
	}
	let { publicationDate, content }: Props = $props();
</script>

<div>
	//оси всегда отображаются
	{#if content.isLoading}
		..
	{:else}
		...
	{/if}
</div>
