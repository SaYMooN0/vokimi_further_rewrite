<script lang="ts">
	import type { PageProps } from './$types';
	import DefaultGeneralVokiTaking from './DefaultGeneralVokiTaking.svelte';
	import CouldNotContinueExistingSession from '../_c_shared/CouldNotContinueExistingSession.svelte';
	import PageLoadErrView from '$lib/components/PageLoadErrView.svelte';

	let { data }: PageProps = $props();
	console.log(data);
</script>

{#if data.sessionActionResult === 'ContinueErr:NoSessionId'}
	<CouldNotContinueExistingSession vokiId={data.vokiId} />
{:else if !data.response.isSuccess}
	<PageLoadErrView
		defaultMessage="Unable to load voki data"
		errs={data.response.errs}
		additionalParams={[
			{ name: 'Voki id', value: data.vokiId },
			{ name: 'Voki type', value: data.vokiType }
		]}
	/>
{:else if data.sessionActionResult === 'NewStarted' && !data.response.data.newSessionStarted}
	<DefaultGeneralVokiTaking
		takingData={data.response.data}
		clearVokiSeenUpdateTimer={() => {}}
		onResultReceived={() => {}}
	/>
{/if}
