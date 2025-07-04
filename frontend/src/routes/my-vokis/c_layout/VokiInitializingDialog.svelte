<script lang="ts">
	import { goto } from '$app/navigation';
	import DialogWithCloseButton from '$lib/components/dialogs/DialogWithCloseButton.svelte';
	import DefaultErrBlock from '$lib/components/errs/DefaultErrBlock.svelte';
	import PrimaryButton from '$lib/components/PrimaryButton.svelte';
	import { ApiVokiCreationCore } from '$lib/ts/backend-services';
	import type { Err } from '$lib/ts/err';
	import { StringUtils } from '$lib/ts/string-utils';
	import type { VokiType } from '$lib/ts/voki';
	import VokiTypeCard from './c_initializing_dialog/VokiTypeCard.svelte';

	let dialog = $state<DialogWithCloseButton>()!;
	let selectedVokiType = $state<VokiType>('general');
	let vokiName = $state('');
	let errs: Err[] = $state([]);

	export function open() {
		errs = [];
		dialog.open();
	}

	async function submitCreate() {
		validateForm();
		if (errs.length > 0) {
			return;
		}
		const response = await ApiVokiCreationCore.fetchJsonResponse<{ initializedVokiId: string }>(
			'/initialize-new-voki',
			ApiVokiCreationCore.requestJsonOptions({ newVokiName: vokiName, vokiType: selectedVokiType })
		);
		if (response.isSuccess) {
			goto(`/voki-creation/${response.data.initializedVokiId}`);
		} else {
			errs = response.errs;
		}
	}
	function validateForm(): Err[] {
		errs = [];
		if (StringUtils.isNullOrWhiteSpace(vokiName)) {
			errs.push({ message: 'Voki name cannot be empty' });
		}
		if (selectedVokiType != 'general') {
			errs.push({ message: 'Unsupported voki type' });
		}
		return errs;
	}
</script>

<DialogWithCloseButton dialogId="voki-initializing-dialog" bind:this={dialog}>
	<p class="subheading">Choose new Voki type</p>
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
	<p class="subheading">Input Voki name</p>
	<input bind:value={vokiName} class="name-input" type="text" placeholder="Voki name..." />
	<DefaultErrBlock errList={errs} />

	<PrimaryButton onclick={() => submitCreate()}>Create</PrimaryButton>
</DialogWithCloseButton>

<style>
	:global(#voki-initializing-dialog .dialog-content) {
		display: flex;
		align-items: center;
		flex-direction: column;
		padding: 0 4rem;
	}
	.subheading {
		margin: 2.5rem 0 1rem 0;
		font-size: 2rem;
		font-weight: 500;
		width: fit-content;
	}

	.voki-type-container {
		display: flex;
		flex-direction: row;
		gap: 4rem;
	}

	.name-input {
		width: 100%;
		height: 2.5rem;
		border: none;
		border-radius: 0.5rem;
		background-color: var(--secondary);
		color: var(--text);
		box-shadow: var(--shadow);

		font-size: 1.5rem;
		font-weight: 420;
		letter-spacing: 0.25px;
		padding: 0.125rem 0.5rem;
		border: 0.2rem solid transparent;
		transition: background-color 0.08s ease-in-out;
	}
	.name-input:focus {
		outline: none;
		border-color: var(--primary);
	}
	:global(#voki-initializing-dialog .err-block) {
		margin: 1rem 1rem;
	}
	:global(#voki-initializing-dialog .primary-btn) {
		margin: auto auto 2rem auto;
		padding: 0.25rem 1.5rem;
	}
</style>
