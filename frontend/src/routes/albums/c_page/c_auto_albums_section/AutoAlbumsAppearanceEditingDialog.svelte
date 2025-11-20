<script lang="ts">
	import PrimaryButton from '$lib/components/buttons/PrimaryButton.svelte';
	import DialogWithCloseButton from '$lib/components/dialogs/DialogWithCloseButton.svelte';
	import type { Err } from '$lib/ts/err';
	import { watch } from 'runed';
	import type { AutoAlbumsColorsPair } from '../../types';
	import AutoAlbumColorEditor from './c_auto_albums_appearance_dialog/AutoAlbumColorEditor.svelte';
	import DefaultErrBlock from '$lib/components/errs/DefaultErrBlock.svelte';

	interface Props {
		takenVokisAlbumsColors: AutoAlbumsColorsPair;
		ratedVokisAlbumsColors: AutoAlbumsColorsPair;
		commentedVokisAlbumsColors: AutoAlbumsColorsPair;
	}

	let { takenVokisAlbumsColors, ratedVokisAlbumsColors, commentedVokisAlbumsColors }: Props =
		$props();

	let takenEditingColors: AutoAlbumsColorsPair = $state(takenVokisAlbumsColors);
	let ratedEditingColors: AutoAlbumsColorsPair = $state(ratedVokisAlbumsColors);
	let commentedEditingColors: AutoAlbumsColorsPair = $state(commentedVokisAlbumsColors);

	let dialog = $state<DialogWithCloseButton>()!;

	export function open() {
		//set to initial
		takenEditingColors = takenVokisAlbumsColors;
		ratedEditingColors = ratedVokisAlbumsColors;
		commentedEditingColors = commentedVokisAlbumsColors;
		dialog.open();
	}
	let isLoading = $state(false);
	let errs: Err[] = $state([]);
	watch(
		[
			() => takenEditingColors.mainColor,
			() => takenEditingColors.secondaryColor,
			() => ratedEditingColors.mainColor,
			() => ratedEditingColors.secondaryColor,
			() => commentedEditingColors.mainColor,
			() => commentedEditingColors.secondaryColor
		],
		() => {
			errs = [];
		}
	);
	async function save() {
		errs = [{ message: 'Not implemented' }];
		// const response=;
	}
</script>

<DialogWithCloseButton
	bind:this={dialog}
	dialogId="auto-albums-editing-dialog"
	subheading="Auto albums appearance editing"
>
	<div class="albums-grid">
		<AutoAlbumColorEditor
			title="Taken Vokis"
			descriptionVokiAction="have taken"
			iconId="#albums-taken-vokis-icon"
			bind:colors={takenEditingColors}
		/>
		<div class="columns-sep"></div>
		<AutoAlbumColorEditor
			title="Rated Vokis"
			descriptionVokiAction="have rated"
			iconId="#albums-rated-vokis-icon"
			bind:colors={ratedEditingColors}
		/>
		<div class="columns-sep"></div>
		<AutoAlbumColorEditor
			title="Commented Vokis"
			descriptionVokiAction="have commented"
			iconId="#albums-commented-vokis-icon"
			bind:colors={commentedEditingColors}
		/>
	</div>
	<DefaultErrBlock errList={errs} />
	<PrimaryButton onclick={() => save()} class={isLoading ? 'unselectable loading' : 'unselectable'}
		>{isLoading ? 'Saving...' : 'Save'}</PrimaryButton
	>
</DialogWithCloseButton>

<style>
	.albums-grid {
		display: grid;
		grid-template-columns: 1fr auto 1fr auto 1fr;
		gap: 2.5rem;
		padding: 0 2rem;
	}
	:global(#auto-albums-editing-dialog .err-block) {
		margin-top: 2rem;
		min-height: 2rem;
		margin-bottom: 1rem;
	}
	.columns-sep {
		align-self: center;
		width: 0.125rem;
		height: 100%;
		border-radius: 0.125rem;
		background-color: var(--muted);
	}
	:global(#auto-albums-editing-dialog .primary-btn) {
		width: 15rem;
		font-weight: 475;
		border-radius: 0.375rem;
		letter-spacing: 1px;
	}
	:global(#auto-albums-editing-dialog .primary-btn.loading) {
		opacity: 0.8;
		transform: none !important;
		cursor: not-allowed;
	}
</style>
