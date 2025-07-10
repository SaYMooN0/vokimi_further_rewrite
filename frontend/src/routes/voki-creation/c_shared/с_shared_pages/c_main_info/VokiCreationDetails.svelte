<script lang="ts">
	import type { VokiDetails } from '../../../voki-creation-page-types';

	const { details }: { details: VokiDetails } = $props<{ details: VokiDetails }>();
</script>

<h1>{details.description}</h1>
<h1>{details.language}</h1>
<h1>{details.isAgeRestricted}</h1>
