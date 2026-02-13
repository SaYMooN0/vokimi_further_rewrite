<script lang="ts">
	import type { PageProps } from './$types';
	import PageLoadErrView from '$lib/components/PageLoadErrView.svelte';
	import UnfinishedSessionExists from './_c_page/UnfinishedSessionExists.svelte';
	import GeneralVokiTaking from './_c_page/GeneralVokiTaking.svelte';
	import ContinueErrNoSessionId from '../_c_shared/ContinueErrNoSessionId.svelte';
	import TerminateErrNoSessionId from '../_c_shared/TerminateErrNoSessionId.svelte';

	let { data }: PageProps = $props();
</script>

{#if !data.isSuccess}
	<PageLoadErrView errs={data.errs} />
{:else if data.data.serverResultType === 'TerminateErr:NoSessionId'}
	<TerminateErrNoSessionId vokiId={data.vokiId} />
{:else if data.data.serverResultType === 'ContinueErr:NoSessionId'}
	<ContinueErrNoSessionId vokiId={data.vokiId} />
{:else if data.data.serverResultType === 'StartNewErr:UnfinishedSessionExists'}
	<UnfinishedSessionExists {...data.data.sessionData} />
{:else if data.data.serverResultType === 'Success'}
	<GeneralVokiTaking sessionData={data.data.sessionData} saveData={data.data.savedData} />
{:else}
	<h1>Something went wrong</h1>
	<a href={`/catalog/${data.vokiId}`}>Go to catalog page</a>
{/if}
