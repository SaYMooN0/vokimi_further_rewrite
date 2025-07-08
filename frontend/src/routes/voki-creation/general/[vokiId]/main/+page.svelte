<script lang="ts">
	import DefaultErrBlock from '$lib/components/errs/DefaultErrBlock.svelte';
	import UnableToLoad from '../../../c_shared/UnableToLoad.svelte';
	import type { PageProps } from './$types';

	let { data }: PageProps = $props();
</script>

{#if !data.isSuccess}
	<UnableToLoad errs={data.errs} />
{:else}
	<h1>Main page</h1>
	name cover details: description language is-age-restricted tags
{/if}
