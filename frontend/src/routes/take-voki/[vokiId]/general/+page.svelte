<script lang="ts">
	import type { PageProps } from './$types';
	import PageLoadErrView from '$lib/components/PageLoadErrView.svelte';
	import SessionIdNotProvided from '../_c_shared/SessionIdNotProvided.svelte';
	import UnfinishedSessionExists from './_c_page/UnfinishedSessionExists.svelte';
	import GeneralVokiTaking from './_c_page/GeneralVokiTaking.svelte';

	let { data }: PageProps = $props();
	console.log(data);
</script>

{#if !data.isSuccess}
	<PageLoadErrView errs={data.errs} />
{:else if data.data.serverResultType === 'ContinueErr:NoSessionId'}
	<SessionIdNotProvided vokiId={data.vokiId} />
{:else if data.data.serverResultType === 'StartNewErr:UnfinishedSessionExists'}
	<UnfinishedSessionExists {...data.data.sessionData} />
{:else if data.data.serverResultType === 'Success'}
	<GeneralVokiTaking sessionData={data.data.sessionData} saveData={data.data.savedData} />
{:else}
	<h1>Something went wrong</h1>
	<a href={`/catalog/${data.vokiId}`}>Go to catalog page</a>
{/if}
