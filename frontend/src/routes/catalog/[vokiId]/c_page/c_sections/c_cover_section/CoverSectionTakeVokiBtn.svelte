<script lang="ts">
	import AuthView from '$lib/components/AuthView.svelte';
	import { StringUtils } from '$lib/ts/utils/string-utils';
	import type { VokiType } from '$lib/ts/voki';
	import AuthNeededToTakeVokiDialog from './c_dialogs/AuthNeededToTakeVokiDialog.svelte';

	interface Props {
		vokiId: string;
		vokiType: VokiType;
		authenticatedOnlyTaking: boolean;
	}
	let { vokiId, vokiType, authenticatedOnlyTaking }: Props = $props();

	let authNeededToTakeVokiDialog = $state<AuthNeededToTakeVokiDialog>()!;
</script>

<AuthNeededToTakeVokiDialog bind:this={authNeededToTakeVokiDialog} />
<AuthView>
	{#snippet children(authState)}
		{#if authenticatedOnlyTaking && !authState.isAuthenticated}
			<button class="take-voki-btn" onclick={() => authNeededToTakeVokiDialog.open()}>
				{@render btnContent()}
			</button>
		{:else}
			<a href="/take-voki/{vokiId}/{StringUtils.pascalToKebab(vokiType)}" class="take-voki-btn">
				{@render btnContent()}
			</a>
		{/if}
	{/snippet}
</AuthView>
{#snippet btnContent()}
	<div class="icon-container">
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
{/snippet}

<style>
	.take-voki-btn {
		background-color: var(--primary);
		color: var(--primary-foreground);
	}

	.take-voki-btn:hover,
	.take-voki-btn:focus {
		background-color: var(--primary-hov);
	}

	.icon-container {
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
