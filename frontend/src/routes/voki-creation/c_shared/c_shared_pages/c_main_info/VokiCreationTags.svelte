<script lang="ts">
	const { tags }: { tags: string[] } = $props<{ tags: string[] }>();
</script>

<h1>
	{#each tags as tag}
		<p>
			{tag}
		</p>
	{/each}
</h1>
