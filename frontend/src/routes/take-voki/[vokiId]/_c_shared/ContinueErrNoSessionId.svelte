<script lang="ts">
	let { vokiId }: { vokiId: string } = $props();
</script>

<div>
	<h1>
		It seems like you wanted to continue an existing Voki taking session, but its data was not
		provided. Please go back to the catalog page and try to continue it from there
	</h1>
	<a href={`/catalog/${vokiId}`}>See the Voki in catalog</a>
</div>
