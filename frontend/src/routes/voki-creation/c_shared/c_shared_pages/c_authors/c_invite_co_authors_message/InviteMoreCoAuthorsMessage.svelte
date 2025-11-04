<script lang="ts">
	import CoAuthorsMessageTemplate from './c_co_authors_msgs_shared/CoAuthorsMessageTemplate.svelte';

	interface Props {
		openInviteDialog: () => void;
		maxCoAuthors: number;
		coAuthorsWithInvitedCount: number;
	}
	
	let { openInviteDialog, maxCoAuthors, coAuthorsWithInvitedCount }: Props = $props();
</script>

<CoAuthorsMessageTemplate
	{subtitle}
	title={null}
	actionItem={{ type: 'button', text: 'Invite more co-author', onClick: openInviteDialog }}
/>
{#snippet subtitle()}
	Voki can have up to {maxCoAuthors} co-authors<br />
	{maxCoAuthors - coAuthorsWithInvitedCount} more can be invited
{/snippet}
