<script lang="ts">
	let { vokiId }: { vokiId: string } = $props();
</script>

<div>
	<h1>
		It seems like you wanted to continue an existing Voki taking session, but something went wrong
	</h1>
	<p>You can try again or start a new session</p>
	<a href={`/catalog/${vokiId}`}>See the Voki in catalog</a>
</div>
