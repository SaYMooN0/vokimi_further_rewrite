<script lang="ts">
	import type { UserProfilePreview } from '$lib/ts/users';

	interface Props {
		isInputEmpty: boolean;
		searchedUsers: UserProfilePreview[];
		isUserAlreadyCoAuthor: (userId: string) => boolean;
		isUserInvitedForCoAuthor: (userId: string) => boolean;
		inviteUser: (userId: string) => Promise<void>;
	}
	let {
		isInputEmpty,
		searchedUsers,
		isUserAlreadyCoAuthor,
		isUserInvitedForCoAuthor,
		inviteUser
	}: Props = $props();
</script>

{#if isInputEmpty}
	<div class="empty-state">
		<svg viewBox="0 0 24 24" fill="none" xmlns="http://www.w3.org/2000/svg">
			<path
				d="M21 12V10C21 7.23858 18.7614 5 16 5H8C5.23858 5 3 7.23858 3 10V10C3 12.7614 5.23858 15 8 15H12"
				stroke="currentColor"
				stroke-linecap="round"
				stroke-linejoin="round"
			/>
			<path
				d="M20.1241 19.1185C20.6654 18.5758 21 17.827 21 17C21 15.3431 19.6569 14 18 14C16.3431 14 15 15.3431 15 17C15 18.6569 16.3431 20 18 20C18.8299 20 19.581 19.663 20.1241 19.1185ZM20.1241 19.1185L22 21"
				stroke="currentColor"
				stroke-linecap="round"
				stroke-linejoin="round"
			/>
		</svg>
		<p class="empty-state-title">Use input above to search for users</p>
		<p class="empty-state-subtitle">Search by display name or @uniqueName</p>
	</div>
{:else if searchedUsers.length === 0}
	<div class="empty-state">
		<svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" fill="none">
			<path
				d="M14 8.5C14 5.73858 11.7614 3.5 9 3.5C6.23858 3.5 4 5.73858 4 8.5C4 11.2614 6.23858 13.5 9 13.5C11.7614 13.5 14 11.2614 14 8.5Z"
				stroke="currentColor"
				stroke-linecap="round"
				stroke-linejoin="round"
			></path>
			<path
				d="M16 20.5C16 16.634 12.866 13.5 9 13.5C5.13401 13.5 2 16.634 2 20.5"
				stroke="currentColor"
				stroke-linecap="round"
				stroke-linejoin="round"
			></path>
			<path
				d="M22 9L19.5 11.5M19.5 11.5L17 14M19.5 11.5L22 14M19.5 11.5L17 9"
				stroke="currentColor"
				stroke-linecap="round"
				stroke-linejoin="round"
			></path>
		</svg>

		<p class="empty-state-title">No matches found</p>
		<p class="empty-state-subtitle">Try to search with other part of the name</p>
	</div>
{:else}
	<div class="users-list">
		{#each searchedUsers as user}
			<div class="user">
				{JSON.stringify(user)}
				<button class="invite-btn" onclick={() => inviteUser(user.id)}>Invite</button>
			</div>
		{/each}
	</div>
{/if}

<style>
	.empty-state {
		display: flex;
		flex-direction: column;
		justify-content: center;
		align-items: center;
		padding: 2rem;
		text-align: center;
		animation: fade-in-from-zero 0.15s ease-in;
	}
	.empty-state > svg {
		height: 4rem;
		width: 4rem;
		stroke-width: 1.5;
		color: var(--primary);
	}
	.empty-state-title {
		color: var(--text);
		font-size: 1.375rem;
		font-weight: 600;
		margin: -0.25rem 0 0.25rem;
	}

	.empty-state-subtitle {
		max-width: 30rem;
		color: var(--muted-foreground);
		font-size: 0.9375rem;
		line-height: 1.5;
	}
    .user{
        background-color: darkblue;
        height: 3rem;
        width: 2rem;
    }
</style>
