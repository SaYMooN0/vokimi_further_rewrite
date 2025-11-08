<script lang="ts">
	import DefaultErrBlock from '$lib/components/errs/DefaultErrBlock.svelte';
	import { StorageBucketMain } from '$lib/ts/backend-communication/storage-buckets';
	import { UsersStore } from '$lib/ts/stores/users-store.svelte';
	import { VokiTypeUtils } from '$lib/ts/voki-type';
	import type { InviteForVokiCoAuthorData } from '../my-voki-invites-page-state.svelte';
	import InviteDisplayInviter from './c_invite_display/InviteDisplayInviter.svelte';

	interface Props {
		invite: InviteForVokiCoAuthorData;
		onAccept: (inviteId: string) => void;
		onDecline: (inviteId: string) => void;
	}

	let { invite, onAccept, onDecline }: Props = $props();

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

<div class="invite-item">
	<div class="left">
		<div class="main">
			<InviteDisplayInviter userId={invite.primaryAuthorId} />

			<p class="invite-text">
				invited you to participate as co-author of "<span class="voki-name">{invite.vokiName}</span
				>" Voki
			</p>

			<div class="meta" aria-label="Voki meta">
				<span class="badge type">
					<svg class="type-icon"><use href={VokiTypeUtils.icon(invite.vokiType)} /></svg>
					{VokiTypeUtils.name(invite.vokiType)}
				</span>
				<span class="count" title="Current co-authors">
					Co-authors count: {invite.coAuthorIds.length}
				</span>

				{#if invite.invitedForCoAuthorUserIds.length > 0}
					<span class="count" title="Pending invitations">
						Invited for co-author: {invite.invitedForCoAuthorUserIds.length}
					</span>
				{/if}

				<span class="date" title={createdOn.toISOString()}>
					{createdOn.toLocaleDateString()}
				</span>
			</div>
		</div>

		<div class="actions">
			<button class="btn primary" onclick={handleAccept}>Accept</button>
			<button class="btn ghost" onclick={handleDecline}>Decline</button>
		</div>
	</div>

	<div class="cover-wrap">
		<img
			class="cover"
			src={StorageBucketMain.fileSrc(invite.vokiCover)}
			alt={`Cover of ${invite.vokiName}`}
		/>
	</div>
</div>

<style>
	.invite-item {
		display: grid;
		grid-template-columns: 1fr 16rem;
		gap: 1.5rem;
		align-items: stretch;
		padding: 1rem 1.5rem;
		border-radius: 1rem;
		background: var(--back);
		box-shadow: var(--shadow-xs) inset;
	}

	.left {
		display: grid;
		grid-template-rows: 1fr auto;
		align-items: start;
		gap: 1rem;
	}

	.main {
		display: grid;
		gap: 0.5rem;
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
		gap: 0.25rem;
		padding: 0.125rem 0.5rem;
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

	.cover-wrap {
		width: 100%;
		height: 100%;
	}

	.cover {
		width: 100%;
		height: 100%;
		object-fit: fill;
		border-radius: var(--voki-cover-border-radius);
		aspect-ratio: var(--voki-cover-aspect-ratio);
		box-shadow: var(--shadow-xs);
	}

	.actions {
		display: flex;
		gap: 0.5rem;
		align-items: center;
	}

	.btn {
		padding: 0.5rem 1rem;
		border-radius: var(--radius);
		cursor: pointer;
		font-weight: 500;
		letter-spacing: 0.125px;
		border: none;
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
		color: var(--text);
		border-color: var(--secondary);
	}
</style>
