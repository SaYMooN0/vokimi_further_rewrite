<script>
	import { page } from '$app/state';
	import ErrView from '$lib/components/errs/ErrView.svelte';
	import PublishingPageComponent from '../../../c_shared/c_shared_pages/PublishingPageComponent.svelte';
</script>

{#if page.params.vokiId}
	<PublishingPageComponent vokiId={page.params.vokiId} />
{:else}
	<ErrView err={{ message: 'Incorrect link. Voki id is not specified' }} />
{/if}
