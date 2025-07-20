<script lang="ts">
	import PrimaryButton from '$lib/components/PrimaryButton.svelte';

	const { openAuthorsInviteDialog }: { openAuthorsInviteDialog: () => void } = $props<{
		openAuthorsInviteDialog: () => void;
	}>();
</script>

<div>
	<PrimaryButton onclick={() => openAuthorsInviteDialog()}>Invite first co-author</PrimaryButton>
</div>
