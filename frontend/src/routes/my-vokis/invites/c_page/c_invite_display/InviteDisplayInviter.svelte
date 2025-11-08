<script lang="ts">
	import DefaultErrBlock from '$lib/components/errs/DefaultErrBlock.svelte';
	import { StorageBucketMain } from '$lib/ts/backend-communication/storage-buckets';
	import { UsersStore } from '$lib/ts/stores/users-store.svelte';
	import { getErrsViewDialogOpenFunction } from '../../../../c_layout/ts_layout_contexts/errs-view-dialog-context';

	interface Props {
		userId: string;
	}
	let { userId }: Props = $props();
	let inviter = UsersStore.Get(userId);
	const openErrsViewDialog = getErrsViewDialogOpenFunction();
</script>

<div
	class="inviter"
	class:loading={inviter.state === 'loading'}
	class:error={inviter.state === 'errs'}
	class:ok={inviter.state === 'ok'}
>
	{#if inviter.state === 'loading'}
		<div class="profile-pic" />
		<div class="names-container-loading">
			<div class="line w1" />
			<div class="line w2" />
		</div>
	{:else if inviter.state === 'errs'}
		<svg class="profile-pic">
			<use href="#common-crossed-circle-icon" />
		</svg>
		<label class="could-not-load-label" onclick={() => openErrsViewDialog(inviter.errs)}
			>Could not load user data<svg><use href="#common-information-icon" /></svg>
		</label>
	{:else}
		<img
			class="profile-pic"
			src={StorageBucketMain.fileSrc(inviter.data.profilePic)}
			alt={inviter.data.displayName}
		/>
		<div class="names-container">
			<div class="display-name">{inviter.data.displayName}</div>
			<a class="unique-name" href="/authors/{inviter.data.id}">@{inviter.data.uniqueName}</a>
		</div>
	{/if}
</div>

<style>
	.inviter {
		display: grid;
		grid-template-columns: auto 1fr;
		gap: 0.25rem;
		align-items: center;
		height: 3.5rem;
	}
	.profile-pic {
		aspect-ratio: 1/1;
		height: 3.5rem;
		border-radius: 50%;
		object-fit: cover;
		box-shadow: var(--shadow-xs);
	}
	.names-container {
		display: grid;
		gap: 0;
	}
	.display-name {
		color: var(--text);
		font-weight: 600;
		font-size: 1.375rem;
		white-space: nowrap;
		overflow: hidden;
		text-overflow: ellipsis;
		cursor: default;
	}
	.unique-name {
		color: var(--muted-foreground);
		font-size: 1rem;
		white-space: nowrap;
		overflow: hidden;
		text-overflow: ellipsis;
	}
	.unique-name:hover {
		color: var(--primary);
	}
	.loading .profile-pic {
		background-color: var(--secondary);
		box-shadow: var(--shadow-xs);
	}
	.names-container-loading {
		display: grid;
		gap: 0.5rem;
	}

	.line {
		background: var(--secondary);
		border-radius: 0.375rem;
		box-shadow: var(--shadow-xs);
	}
	.w1 {
		width: 12rem;
		height: 1.125rem;
	}
	.w2 {
		height: 1rem;
		width: 7.5rem;
	}
	.inviter.error > .profile-pic {
		background-color: var(--secondary);
		box-shadow: var(--shadow-xs);
	}
	.could-not-load-label {
		font-size: 1.125rem;
		font-weight: 475;
		color: var(--secondary-foreground);
		padding: 0.125rem 0.5rem;
		border-radius: 0.5rem;
		width: fit-content;
		cursor: pointer;
	}
	.could-not-load-label > svg {
		color: inherit;
		height: 1.375rem;
		width: 1.375rem;
		stroke-width: 2.25;
		padding: 0;
		vertical-align: middle;
		padding-bottom: 0.125rem;
		margin-left: 0.25rem;
	}
	.could-not-load-label:hover {
		background-color: var(--secondary);
	}
	.inviter.error > svg {
		padding: 0.5rem;
		color: var(--secondary-foreground);
	}
</style>
