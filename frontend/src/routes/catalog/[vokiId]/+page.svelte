<script lang="ts">
	import VokiNotLoaded from "./c_page/VokiNotLoaded.svelte";

	

	let { data }: PageProps = $props();
</script>

{#if !data.isSuccess}
	<VokiNotLoaded errs={data.errs} />
{:else}

{/if}