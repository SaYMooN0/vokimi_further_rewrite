<script lang="ts">
	import type { UserPreviewData } from '$lib/ts/stores/users-store-svelte';

	let { user }: { user: UserPreviewData } = $props<{ user: UserPreviewData }>();
</script>

<a class="auhtor-view" href="/authors/{user.id}"> </a>
