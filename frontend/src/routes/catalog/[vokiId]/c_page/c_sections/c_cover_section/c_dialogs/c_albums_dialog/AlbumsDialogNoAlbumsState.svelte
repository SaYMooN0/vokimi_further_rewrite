<script lang="ts">
	import PrimaryButton from '$lib/components/buttons/PrimaryButton.svelte';
	interface Props {
		changeToCreateNewAlbum: () => void;
	}
	let { changeToCreateNewAlbum }: Props = $props();
</script>

<div class="no-albums">
	<PrimaryButton onclick={() => changeToCreateNewAlbum()}>Create first album</PrimaryButton>
</div>

<style>
	.no-albums {
		display: flex;
		flex-direction: column;
		gap: 1rem;
	}
</style>
