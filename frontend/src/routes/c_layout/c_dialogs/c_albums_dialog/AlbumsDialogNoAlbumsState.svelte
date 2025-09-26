<script lang="ts">
	import PrimaryButton from '$lib/components/buttons/PrimaryButton.svelte';

	let { createNew }: { createNew: () => void } = $props<{ createNew: () => void }>();
</script>

<div class="no-albums">
	<PrimaryButton onclick={createNew}>Create first album</PrimaryButton>
</div>
<style>
	.no-albums {
		display: flex;
		flex-direction: column;
		gap: 1rem;
	}
</style>
