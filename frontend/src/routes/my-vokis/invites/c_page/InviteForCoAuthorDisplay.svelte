<script lang="ts">
	import DefaultErrBlock from '$lib/components/errs/DefaultErrBlock.svelte';
	import { StorageBucketMain } from '$lib/ts/backend-communication/storage-buckets';
	import { UsersStore } from '$lib/ts/stores/users-store.svelte';
	import { VokiTypeUtils } from '$lib/ts/voki-type';
	import type { InviteForVokiCoAuthorData } from '../my-voki-invites-page-state.svelte';

	interface Props {
		invite: InviteForVokiCoAuthorData;
		onAccept: (inviteId: string) => void;
		onDecline: (inviteId: string) => void;
	}

	let { invite, onAccept, onDecline }: Props = $props();

	let inviter = UsersStore.Get(invite.primaryAuthorId);

	const createdOn =
		invite.creationDate instanceof Date
			? invite.creationDate
			: new Date(invite.creationDate as unknown as string);

	const invitedCount = invite.invitedForCoAuthorUserIds?.length ?? 0;
	function handleAccept() {
		onAccept(invite.vokiId);
	}
	function handleDecline() {
		onDecline(invite.vokiId);
	}
</script>

<article class="invite-item">
	<div class="main">
		{#if inviter.state === 'loading'}
			<div class="inviter loading" aria-busy="true" aria-label="Loading inviter profile">
				<div class="profile-pic-skeleton" />
				<div class="lines">
					<div class="line w1" />
					<div class="line w2" />
				</div>
			</div>
		{:else if inviter.state === 'errs'}
			<div class="inviter errs">
				<DefaultErrBlock errList={inviter.errs} />
			</div>
		{:else}
			<div class="inviter ok">
				<img
					class="profile-pic"
					src={StorageBucketMain.fileSrc(inviter.data.profilePic)}
					alt={inviter.data.displayName}
					loading="lazy"
				/>
				<div class="names-container">
					<div class="display-name">{inviter.data.displayName}</div>
					<div class="unique-name">@{inviter.data.uniqueName}</div>
				</div>
			</div>

			<p class="invite-text">
				invited you to participate as co-author of "<span class="voki-name">{invite.vokiName}</span
				>" Voki
			</p>

			<div class="meta" aria-label="Voki meta">
				<span class="badge type">
					<svg class="type-icon"><use href={VokiTypeUtils.icon(invite.vokiType)} /></svg>
					{VokiTypeUtils.name(invite.vokiType)}
				</span>
				<span class="count" title="Current co-authors"
					>Co-authors count: {invite.coAuthorIds.length}</span
				>

				{#if invite.invitedForCoAuthorUserIds.length > 0}
					<span class="count" title="Pending invitations"
						>Invited for co-author: {invite.invitedForCoAuthorUserIds.length}</span
					>
				{/if}

				<span class="date" title={createdOn.toISOString()}>
					{createdOn.toLocaleDateString()}
				</span>
			</div>
		{/if}
	</div>
	<div class="cover-wrap">
		<img
			class="cover"
			src={StorageBucketMain.fileSrc(invite.vokiCover)}
			alt={`Cover of ${invite.vokiName}`}
			loading="lazy"
		/>
	</div>
	<div class="actions">
		<button class="btn primary" onclick={handleAccept}> Accept </button>
		<button class="btn ghost" onclick={handleDecline}> Decline </button>
	</div>
</article>

<style>
	.invite-item {
		display: grid;
		grid-template-columns: 1fr auto auto;
		gap: 1rem;
		align-items: center;
		padding: 0.5rem 1.5rem;
		border-radius: 1rem;
		background: var(--back);
		box-shadow: var(--shadow-xs) inset;
	}

	.cover-wrap {
		width: 16rem;
	}
	.cover {
		width: 100%;
		aspect-ratio: var(--voki-cover-aspect-ratio);
		object-fit: fill;
		border-radius: var(--voki-cover-border-radius);
		box-shadow: var(--shadow-xs);
	}

	.main {
		display: grid;
		gap: 0.5rem;
	}

	.inviter {
		display: grid;
		grid-template-columns: auto 1fr;
		gap: 0.5rem;
		align-items: center;
	}
	.profile-pic {
		width: 3.5rem;
		height: 3.5rem;
		border-radius: 50%;
		object-fit: cover;
		box-shadow: var(--shadow-xs);
	}
	.names-container {
		display: grid;
		gap: 0.15rem;
	}
	.display-name {
		color: var(--text);
		font-weight: 600;
		font-size: 1.375rem;
		white-space: nowrap;
		overflow: hidden;
		text-overflow: ellipsis;
	}
	.unique-name {
		color: var(--muted-foreground);
		font-size: 1rem;
		white-space: nowrap;
		overflow: hidden;
		text-overflow: ellipsis;
	}

	.invite-text {
		font-size: 1.25rem;
		font-weight: 500;
		color: var(--muted-foreground);
	}
	.invite-text .voki-name {
		font-weight: 600;
		color: var(--text);
	}
	.meta {
		display: inline-flex;
		align-items: center;
		gap: 0.5rem;
		color: var(--muted-foreground);
		font-size: 0.9rem;
		flex-wrap: wrap;
	}

	.badge.type {
		display: inline-flex;
		align-items: center;
		gap: 0.4rem;
		padding: 0.2rem 0.5rem;
		border-radius: 100vw;
		background: var(--secondary);
		color: var(--secondary-foreground);
		box-shadow: var(--shadow-xs);
	}
	.type-icon {
		width: 1.25rem;
		height: 1.25rem;
		stroke-width: 2;
	}
	.count {
		font-weight: 500;
	}

	.actions {
		display: grid;
		gap: 0.5rem;
		align-content: start;
	}
	.btn {
		padding: 0.55rem 0.9rem;
		border-radius: var(--radius);
		border: 0.0625rem solid var(--muted);
		cursor: pointer;
		font-weight: 600;
	}

	.btn.primary {
		background: var(--primary);
		color: var(--primary-foreground);
		border-color: var(--primary);
	}
	.btn.primary:hover:where(:not([disabled])) {
		background: var(--primary-hov);
	}

	.btn.ghost {
		background: var(--back);
		color: var(--text);
	}
	.btn.ghost:hover:where(:not([disabled])) {
		background: var(--secondary);
	}

	.profile-pic-skeleton {
		width: 2.5rem;
		height: 2.5rem;
		border-radius: 50%;
		background: var(--secondary);
		border: 0.0625rem solid var(--muted);
	}
	.lines {
		display: grid;
		gap: 0.35rem;
	}
	.line {
		height: 0.9rem;
		background: var(--secondary);
		border-radius: 0.25rem;
		border: 0.0625rem solid var(--muted);
	}
	.w1 {
		width: 9rem;
	}
	.w2 {
		width: 6rem;
	}
</style>
