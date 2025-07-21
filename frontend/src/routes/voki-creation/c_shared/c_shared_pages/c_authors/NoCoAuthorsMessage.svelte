<script lang="ts">
	import PrimaryButton from '$lib/components/PrimaryButton.svelte';

	const { openAuthorsInviteDialog }: { openAuthorsInviteDialog: () => void } = $props<{
		openAuthorsInviteDialog: () => void;
	}>();
</script>

<div>
	<h1>This voki has no co-authors except you</h1>
	<label>You can invite up to 5 co-authors</label>
	<PrimaryButton onclick={() => openAuthorsInviteDialog()}>Invite first co-author</PrimaryButton>
</div>
