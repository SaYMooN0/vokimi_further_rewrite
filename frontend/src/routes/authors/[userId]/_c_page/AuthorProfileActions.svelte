<script lang="ts">
	import AuthView from '$lib/components/AuthView.svelte';
	import { ApiSubscriptions } from '$lib/ts/backend-communication/backend-services';
	import { AuthStore } from '$lib/ts/stores/auth-store.svelte';
	import { watch } from 'runed';

	interface Props {
		profileId: string;
	}

	let { profileId }: Props = $props();
	let actionsState: 'loading' | 'subscribed' | 'not-subscribed' | 'error' = $state('loading');
	let authState = $state(AuthStore.Get());

	watch([() => authState.name], () => updateActionsState());
	function updateActionsState() {
		if (authState.name === 'loading') {
			actionsState = 'loading';
			return;
		}
		if (authState.name === 'error') {
			actionsState = 'error';
			return;
		}
		ApiSubscriptions.fetchJsonResponse<{ isFollowing: boolean }>(
			`/is-subscribed?userId=${profileId}`,
			{ method: 'GET' }
		).then((response) => {
			if (response.isSuccess) {
				actionsState = response.data.isFollowing ? 'subscribed' : 'not-subscribed';
			} else {
				actionsState = 'error';
			}
		});
	}
</script>

<div class="two-btn-container">
	{#if actionsState === 'error'}
		<div class="main-item error">loading error</div>
		<button class="small-btn" onclick={() => updateActionsState()}>
			<svg class="reload-arrow"><use href="#common-reload-icon" /></svg>
		</button>
	{:else}
		<button class="small-btn"
			><svg class="show-menu-icon"><use href="#common-toggle-content-arrow" /></svg></button
		>
	{/if}
</div>

<style>
	.two-btn-container {
		--small-btn-size: 1.75rem;
		display: grid;
		height: 1.5rem;
		grid-template-columns: 1fr var(--small-btn-size);
		gap: 0.5rem;
	}
	.two-btn-container > * {
		width: 100%;
		height: 100%;
		border-radius: 0.375rem !important;
		border: none;
		padding: 0;
	}
	.main-item {
		display: flex;
		align-items: center;
		justify-content: center;
	}
	.main-item.error {
		background-color: var(--red-2);
		color: var(--red-5);
		font-weight: 450;
		font-size: 1rem;
		cursor: default;
	}
	.small-btn {
		width: var(--small-btn-size);
		height: var(--small-btn-size);
		background-color: var(--muted);
		color: var(--muted-foreground);
	}
	.small-btn svg {
		height: 100%;
		width: 100%;
	}
	svg.show-menu-icon {
		transform: rotate(180deg);
		stroke-width: 1.5;
	}
	.small-btn:hover {
		cursor: pointer;
		background-color: var(--accent);
		color: var(--accent-foreground);
	}
	svg.reload-arrow {
		stroke-width: 2;
		padding: 0.25rem;
		transition: transform 0.2s ease-out;
	}
	.small-btn:has(.reload-arrow):hover .reload-arrow {
		transform: rotate(60deg);
	}
	.small-btn:has(.reload-arrow):active .reload-arrow {
		transform: rotate(180deg);
	}
</style>
