<script lang="ts">
	import type { PageProps } from './$types';
	import UnableToLoad from '../../../../c_shared/UnableToLoad.svelte';

	let { data }: PageProps = $props();
</script>

{#if !data.isSuccess}
	<UnableToLoad errs={data.errs} />
{:else}
	<h1>Question {data.questionId}</h1>
{/if}
