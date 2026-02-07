<script lang="ts">
	import { goto } from '$app/navigation';
	import { navigating } from '$app/state';
	import AuthView from '$lib/components/AuthView.svelte';
	import LinesLoader from '$lib/components/loaders/LinesLoader.svelte';
	import { AuthStore } from '$lib/ts/stores/auth-store.svelte';
	import { StringUtils } from '$lib/ts/utils/string-utils';
	import type { VokiType } from '$lib/ts/voki-type';
	import AuthNeededToTakeVokiDialog from './_c_dialogs/AuthNeededToTakeVokiDialog.svelte';

	interface Props {
		vokiId: string;
		vokiType: VokiType;
		signedInOnlyTaking: boolean;
	}
	let { vokiId, vokiType, signedInOnlyTaking }: Props = $props();

	let authNeededToTakeVokiDialog = $state<AuthNeededToTakeVokiDialog>()!;
	let isBtnOnclickLoading = $state<boolean>(false);
	let isNavigatingToTakeVoki = $derived<boolean>(
		(navigating && navigating.to?.url.pathname.includes(`/take-voki/${vokiId}`)) ?? false
	);
	let showBtnSpinner = $derived<boolean>(isBtnOnclickLoading || isNavigatingToTakeVoki);
	async function onBtnClick(e: MouseEvent) {
		//if not left or middle click
		if (e.button != 0 && e.button != 1) {
			return;
		}

		e.preventDefault();
		isBtnOnclickLoading = true;
		let auth = await AuthStore.GetWithForceRefresh();
		isBtnOnclickLoading = false;

		if (signedInOnlyTaking && !auth.isAuthenticated) {
			authNeededToTakeVokiDialog.open();
			return;
		}
		if (e.button == 1) {
			window.open(`/take-voki/${vokiId}/${StringUtils.pascalToKebab(vokiType)}`, '_blank');
			return;
		}
		goto(`/take-voki/${vokiId}/${StringUtils.pascalToKebab(vokiType)}`);
	}
</script>

<AuthNeededToTakeVokiDialog bind:this={authNeededToTakeVokiDialog} />
<button class="take-voki-btn" onmousedown={(e) => onBtnClick(e)}>
	{#if showBtnSpinner}
		<LinesLoader color="var(--primary-foreground)" sizeRem={1.25} strokePx={2} class="loader" />
		<label class="btn-text">Loading</label>
	{:else}
		<div class="icon-item">
			<svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" fill="none">
				<path
					d="M9.00005 6C9.00005 6 15 10.4189 15 12C15 13.5812 9 18 9 18"
					stroke="currentColor"
					stroke-linecap="round"
					stroke-linejoin="round"
				/>
			</svg>
			<div class="dot"></div>
		</div>
		<label class="btn-text">Take voki</label>
	{/if}
</button>

<style>
	.take-voki-btn {
		background-color: var(--primary);
		color: var(--primary-foreground);
		&:hover,
		&:focus {
			background-color: var(--primary-hov);
		}
	}
	.take-voki-btn > :global(.loader) {
		margin-right: 0.5rem;
	}
	.icon-item {
		position: relative;
		width: auto;
		height: auto;
		transition: all 0.2s ease-out;
	}

	.dot {
		position: absolute;
		top: 50%;
		left: 50%;
		width: 0.25rem;
		height: 0.25rem;
		border-radius: 50%;
		background-color: var(--primary-foreground);
		opacity: 0;
		transition: inherit;
		transform: translate(-30%, -50%);
	}

	.take-voki-btn:hover .dot,
	.take-voki-btn:focus .dot {
		opacity: 1;
		transform: translate(-120%, -50%);
	}

	svg {
		transition: inherit;
	}

	.take-voki-btn:hover svg,
	.take-voki-btn:focus svg {
		transform: translateX(3px);
	}
</style>
