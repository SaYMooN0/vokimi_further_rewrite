<script lang="ts">
	import PageLoadErrView from '$lib/components/PageLoadErrView.svelte';
	import { toast } from 'svelte-sonner';
	import type { PageProps } from './$types';
	import ProfileSetupProcess from './c_page/ProfileSetupProcess.svelte';

	let { data }: PageProps = $props();
	let setupState: 'process' | 'complete' = $state('process');
	async function saveChanges() {
		toast.error("Sorry, this feature isn't implemented yet");
	}
</script>

{#if !data.isSuccess}
	<PageLoadErrView errs={data.errs} defaultMessage="Could not load vokis catalog" />
{:else}
	<ProfileSetupProcess />
{/if}
