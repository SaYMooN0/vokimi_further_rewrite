<script lang="ts">
	import AuthorsPageComponent from '../../../c_shared/c_shared_pages/AuthorsPageComponent.svelte';
	import UnableToLoad from '../../../c_shared/UnableToLoad.svelte';
	import type { PageProps } from './$types';

	let { data }: PageProps = $props();
</script>

{#if !data.isSuccess}
	<UnableToLoad errs={data.errs} />
{:else}
	<AuthorsPageComponent vokiId={data.vokiId!} {...data.data} />
{/if}
