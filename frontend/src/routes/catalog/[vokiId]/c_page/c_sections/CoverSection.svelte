<script lang="ts">
	import AuthView from '$lib/components/AuthView.svelte';
	import { StorageBucketMain } from '$lib/ts/backend-communication/storage-buckets';
	import type { VokiType } from '$lib/ts/voki';
	import CoverSectionAddToAlbum from './c_cover_section_buttons/CoverSectionAddToAlbumBtn.svelte';
	import CoverSectionAlbumsBlockedBtn from './c_cover_section_buttons/CoverSectionAlbumsBlockedBtn.svelte';
	import CoverSectionManageVokiBtn from './c_cover_section_buttons/CoverSectionManageVokiBtn.svelte';
	import CoverSectionTakeVokiBtn from './c_cover_section_buttons/CoverSectionTakeVokiBtn.svelte';
	interface Props {
		vokiId: string;
		cover: string;
		usersWithAccessToManage: string[];
		vokiType: VokiType;
	}
	let { vokiId, cover, usersWithAccessToManage, vokiType }: Props = $props();
</script>

<div class="voki-cover-section">
	<img class="voki-cover" src={StorageBucketMain.fileSrc(cover)} alt="voki cover" />
	<div class="buttons-container">
		<CoverSectionTakeVokiBtn {vokiId} {vokiType} />
		<AuthView>
			{#snippet authenticated(authData)}
				<CoverSectionAddToAlbum {vokiId} />
				{#if usersWithAccessToManage.includes(authData.userId)}
					<CoverSectionManageVokiBtn {vokiId} />
				{/if}
			{/snippet}
			{#snippet unauthenticated()}
				<CoverSectionAlbumsBlockedBtn />
			{/snippet}
		</AuthView>
	</div>
</div>

<style>
	.voki-cover-section {
		display: flex;
		flex-direction: column;
		justify-content: center;
		align-items: center;
		gap: 1rem;
		width: 100%;
		align-self: flex-start;
	}

	.voki-cover-section > img {
		width: 25rem;
		border-radius: var(--voki-cover-border-radius);
		aspect-ratio: var(--voki-cover-aspect-ratio);
		box-shadow: var(--shadow-xs);
	}

	.buttons-container {
		display: flex;
		flex-direction: column;
		gap: 0.5rem;
		width: 100%;
	}

	.buttons-container > :global(*) {
		display: flex;
		flex-direction: row;
		justify-content: center;
		align-items: center;
		width: 100%;
		height: 2rem;
		border: none;
		border-radius: 0.25rem;
		cursor: default;
	}
	.buttons-container > :global(*:focus) {
		outline: var(--primary) solid 0.125rem;
	}

	.buttons-container > :global(* > svg) {
		width: 1.75rem;
		height: 1.875rem;
		margin-right: 0.5rem;
		stroke-width: 2;
	}

	.buttons-container > :global(* > .btn-text) {
		width: 8.5rem;
		font-size: 1.25rem;
		cursor: inherit;
		font-weight: 450;
		letter-spacing: 0.2px;
		text-align: start;
	}
</style>
