<script lang="ts">
	import PageLoadErrView from '$lib/components/PageLoadErrView.svelte';
	import { toast } from 'svelte-sonner';
	import type { PageProps } from './$types';
	import ProfileSetupProcess from './c_page/ProfileSetupProcess.svelte';
	import SetupSavedMessage from './c_page/SetupSavedMessage.svelte';
	import type { ResponseVoidResult } from '$lib/ts/backend-communication/result-types';

	let { data }: PageProps = $props();
	let setupState: 'process' | 'complete' = $state('process');
	async function saveChanges(): Promise<ResponseVoidResult> {
		toast.error("Sorry, this feature isn't implemented yet");
		return { isSuccess: false, errs: [] };
	}
</script>

{#if !data.isSuccess}
	<PageLoadErrView errs={data.errs} defaultMessage="Could not load vokis catalog" />
{:else if setupState === 'process'}
	<ProfileSetupProcess
		initialLangs={data.data.preferredLanguages}
		initialTags={data.data.favoriteTags}
		initialProfilePic={data.data.profilePicture}
		initialDisplayName={data.data.displayName}
		saveSetup={saveChanges}
	/>
{:else if setupState === 'complete'}
	<SetupSavedMessage />
{:else}
	<h1>Something went wrong. Reload the page</h1>
{/if}
