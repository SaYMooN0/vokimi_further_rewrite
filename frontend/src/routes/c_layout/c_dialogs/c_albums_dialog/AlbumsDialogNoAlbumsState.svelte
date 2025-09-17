<script lang="ts">
	import PrimaryButton from '$lib/components/buttons/PrimaryButton.svelte';

	let { createNew }: { createNew: () => void } = $props<{ createNew: () => void }>();
</script>

<div class="no-albums">
	<label>You have not created any albums yet</label>
	<PrimaryButton onclick={createNew}>Create first Vokis album</PrimaryButton>
</div>
