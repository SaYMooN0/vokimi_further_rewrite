<script lang="ts">
	import type {
		UserInviteState,
		UserPreviewWithInvitesSettings,
		UsersRecommendedToInvite
	} from '../../types';
	import SearchedUsersList from './SearchedUsersList.svelte';

	interface Props {
		usersRecommendedToInvite: UsersRecommendedToInvite;
		getUserInviteState: (user: UserPreviewWithInvitesSettings) => UserInviteState;
	}
	let { usersRecommendedToInvite, getUserInviteState }: Props = $props();
</script>

<div class="input-empty-state">
	{#if usersRecommendedToInvite.state === 'errs'}
		<div class="err-container">Could not load users to recommend</div>
	{:else if usersRecommendedToInvite.state === 'loading'}
		<div class="loading-container">Loading users to recommend</div>
	{:else if usersRecommendedToInvite.state === 'ok'}
		<SearchedUsersList users={usersRecommendedToInvite.users} {getUserInviteState} />
	{/if}
	<div class="notes">
		<p class="title">If you want to invite specific user use input above to search for them</p>
		<p class="subtitle">
			Search by <span class="search-val">display name</span> or
			<span class="search-val">@uniqueName</span>
		</p>
	</div>
</div>

<style>
	.input-empty-state {
		height: 100%;
		display: flex;
		flex-direction: column;
		gap: 0.25rem;
	}
	.notes {
		background-color: var(--secondary);
		padding: 0.25rem;
		border-radius: 0.5rem;
	}
	.title {
		color: var(--secondary-foreground);
		font-size: 1rem;
		line-height: 1.25rem;
		text-align: center;
		font-weight: 450;
	}

	.subtitle {
		color: var(--secondary-foreground);
		font-size: 0.875rem;
		line-height: 1rem;
		text-align: center;
		font-weight: 450;
	}
	.search-val {
		font-weight: 550;
		text-decoration: underline;
	}
</style>
