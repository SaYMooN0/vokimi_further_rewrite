<script lang="ts">
	import PrimaryButton from '$lib/components/buttons/PrimaryButton.svelte';
	import DialogWithCloseButton from '$lib/components/dialogs/DialogWithCloseButton.svelte';
	import { toast } from 'svelte-sonner';
	import type { VokiAlbumPreviewData } from '../../types';
	interface Props {
		userAlbums: VokiAlbumPreviewData[];
	}
	let { userAlbums }: Props = $props();
	let destination: VokiAlbumPreviewData | undefined = $state(undefined)!;
	let dialog = $state<DialogWithCloseButton>()!;
	let isLoading = $state(false)!;

	export function open(a: VokiAlbumPreviewData) {
        console.log(a);
		destination = a;
		dialog.open();
	}
	async function save() {
		dialog.close();
		toast.error('This feature is not implemented');
	}
</script>

<DialogWithCloseButton
	bind:this={dialog}
	dialogId="copy-vokis-from-another-album-dialog"
	onBeforeClose={() => (destination = undefined)}
>
	{#if destination}
		<h1>Choose albums you want to copy Vokis from</h1>
		<div class="vokis-container">
			<p class="type-subheader">User albums</p>
			{#each userAlbums as album}
				<div>{album.vokisCount}</div>
			{/each}
		</div>
		<PrimaryButton onclick={() => save()} class={isLoading ? 'loading' : ''}
			>{#if isLoading}Saving...{:else}Save{/if}</PrimaryButton
		>
	{:else}
		<p class="no-album-selcted">No destination album selected</p>
	{/if}
</DialogWithCloseButton>
