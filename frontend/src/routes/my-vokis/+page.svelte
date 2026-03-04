<script lang="ts">
	import { goto } from '$app/navigation';
	import { onMount } from 'svelte';
	onMount(() => {
		goto(`/my-vokis/draft-vokis`);
	});
</script>
