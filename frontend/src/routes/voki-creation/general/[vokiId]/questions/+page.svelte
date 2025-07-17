<script lang="ts">
	import UnableToLoad from '../../../c_shared/UnableToLoad.svelte';
	import type { PageProps } from './$types';

	let { data }: PageProps = $props();
</script>

{#if !data.isSuccess}
	<UnableToLoad errs={data.errs} />
{:else}
	<div>{JSON.stringify(data.data)}</div>
{/if}
