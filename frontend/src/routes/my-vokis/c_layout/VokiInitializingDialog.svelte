<script lang="ts">
	import DialogWithCloseButton from '$lib/components/dialogs/DialogWithCloseButton.svelte';
	import type { VokiType } from '$lib/ts/voki';
	import VokiTypeCard from './c_initializing_dialog/VokiTypeCard.svelte';

	let dialog = $state<DialogWithCloseButton>()!;
	let selectedVokiType = $state<VokiType>('general');
	export function open() {
		dialog.open();
	}
</script>

<DialogWithCloseButton dialogId="voki-initializing-dialog" bind:this={dialog}>
	<h1>Choose new Voki type</h1>
	<div class="voki-type-container">
		<VokiTypeCard
			name="General"
			type="general"
			isSelected={selectedVokiType === 'general'}
			onclick={() => (selectedVokiType = 'general')}
		/>
		<VokiTypeCard
			name="Tier List"
			type="tier-list"
			isSelected={selectedVokiType === 'tier-list'}
			onclick={() => (selectedVokiType = 'tier-list')}
		/>
		<VokiTypeCard
			name="Scoring"
			type="scoring"
			isSelected={selectedVokiType === 'scoring'}
			onclick={() => (selectedVokiType = 'scoring')}
		/>
	</div>
</DialogWithCloseButton>

<style>
	h1 {
		margin-bottom: 2rem;
		text-align: center;
	}

	.voki-type-container {
		display: flex;
		flex-direction: row;
		gap: 2rem;
	}
   
</style>
