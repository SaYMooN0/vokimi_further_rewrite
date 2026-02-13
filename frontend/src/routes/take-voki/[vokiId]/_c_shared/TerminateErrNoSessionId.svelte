<script lang="ts">
	let { vokiId }: { vokiId: string } = $props();
</script>

<div>
	<h1>
		It seems like you wanted to terminate an existing Voki taking session and start a new one, but
		something went wrong. Please go back to the catalog page and try again from there
	</h1>
	<a href={`/catalog/${vokiId}`}>See the Voki in catalog</a>
</div>
