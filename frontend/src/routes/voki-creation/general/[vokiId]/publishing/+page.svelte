<script>
	import { page } from '$app/state';
	import PublishingPageComponent from '../../../c_shared/c_shared_pages/PublishingPageComponent.svelte';
</script>

<PublishingPageComponent vokiId={page.params.vokiId} />
