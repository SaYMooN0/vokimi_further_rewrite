<script lang="ts">
	import type { UserPreviewWithInvitesSettings } from '../../types';
	import SearchedUserInListItem from './c_users_list/SearchedUserInListItem.svelte';

	interface Props {
		isInputEmpty: boolean;

		getUserInviteStateForVoki: (
			userId: string
		) => 'PrimaryAuthor' | 'CoAuthor' | 'AlreadyInvited' | 'CandidateToInvite';
		userOptions: UserPreviewWithInvitesSettings[];
		isUserInListToInvite: (userId: string) => boolean;
		addToListToInvite: (user: UserPreviewWithInvitesSettings) => void;
		removeFromListToInvite: (user: UserPreviewWithInvitesSettings) => void;
	}
	let {
		isInputEmpty,
		getUserInviteStateForVoki,
		userOptions,
		isUserInListToInvite,
		addToListToInvite,
		removeFromListToInvite
	}: Props = $props();

	function getUserInviteState(user: UserPreviewWithInvitesSettings): UserItemInListState {
		const currentState = getUserInviteStateForVoki(user.id);
		if (currentState === 'PrimaryAuthor') {
			return { state: 'PrimaryAuthor' };
		} else if (currentState === 'CoAuthor') {
			return { state: 'CoAuthor' };
		} else if (currentState === 'AlreadyInvited') {
			return { state: 'AlreadyInvited' };
		} else {
			return {
				state: 'CandidateToInvite',
				isUserInListToInvite: isUserInListToInvite(user.id),
				addToListToInvite: () => addToListToInvite(user),
				removeFromListToInvite: () => removeFromListToInvite(user)
			};
		}
	}
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
{:else if userOptions.length === 0}
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
	<div class="users-list-wrapper">
		<div class="fade-top"></div>

		<div class="users-list">
			{#each userOptions as user}
				<SearchedUserInListItem
					uniqueName={user.uniqueName}
					displayName={user.displayName}
					profilePic={user.profilePic}
					badge={getUserInviteState(user)}
				/>
			{/each}
		</div>

		<div class="fade-bottom"></div>
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
		width: 4rem;
		height: 4rem;
		color: var(--primary);
		stroke-width: 1.5;
	}

	.empty-state-title {
		margin: -0.25rem 0 0.25rem;
		color: var(--text);
		font-size: 1.375rem;
		font-weight: 600;
	}

	.empty-state-subtitle {
		max-width: 30rem;
		color: var(--muted-foreground);
		font-size: 0.9375rem;
		line-height: 1.5;
	}

	.users-list-wrapper {
		position: relative;
		display: flex;
		flex-direction: column;
		height: 100%;
	}

	.users-list {
		display: flex;
		flex-direction: column;
		gap: 0.5rem;
		height: 100%;
		padding: 0.375rem 0;
		overflow-y: auto;
		scrollbar-gutter: stable;
	}

	.fade-top,
	.fade-bottom {
		position: absolute;
		right: 0;
		left: 0;
		z-index: 2;
		height: 1rem;
		pointer-events: none;
	}

	.fade-top {
		top: -1px;
		background: linear-gradient(var(--back), transparent);
	}

	.fade-bottom {
		bottom: -1px;
		background: linear-gradient(transparent, var(--back));
	}
</style>
