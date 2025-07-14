<script lang="ts">
	import { goto } from '$app/navigation';
	import DialogWithCloseButton from '$lib/components/dialogs/DialogWithCloseButton.svelte';
	import DefaultErrBlock from '$lib/components/errs/DefaultErrBlock.svelte';
	import PrimaryButton from '$lib/components/PrimaryButton.svelte';
	import { ApiVokiCreationCore } from '$lib/ts/backend-communication/backend-services';
	import type { Err } from '$lib/ts/err';
	import { RequestJsonOptions } from '$lib/ts/request-json-options';
	import { StringUtils } from '$lib/ts/utils/string-utils';
	import type { VokiType } from '$lib/ts/voki';
	import VokiTypeCard from './c_initializing_dialog/VokiTypeCard.svelte';

	let dialog = $state<DialogWithCloseButton>()!;
	let selectedVokiType = $state<VokiType>('General');
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
		const response = await ApiVokiCreationCore.fetchJsonResponse<{ id: string; type: VokiType }>(
			'/initialize-new-voki',
			RequestJsonOptions.POST({ newVokiName: vokiName, vokiType: selectedVokiType })
		);
		if (response.isSuccess) {
			const type = StringUtils.pascalToKebab(response.data.type);
			goto(`/voki-creation/${type}/${response.data.id}`);
		} else {
			errs = response.errs;
		}
	}
	function validateForm(): Err[] {
		errs = [];
		if (StringUtils.isNullOrWhiteSpace(vokiName)) {
			errs.push({ message: 'Voki name cannot be empty' });
		}
		if (selectedVokiType != 'General') {
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
			type="General"
			isSelected={selectedVokiType === 'General'}
			onclick={() => (selectedVokiType = 'General')}
		/>
		<VokiTypeCard
			name="Tier List"
			type="TierList"
			isSelected={selectedVokiType === 'TierList'}
			onclick={() => (selectedVokiType = 'TierList')}
		/>
		<VokiTypeCard
			name="Scoring"
			type="Scoring"
			isSelected={selectedVokiType === 'Scoring'}
			onclick={() => (selectedVokiType = 'Scoring')}
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
		flex-direction: column;
		align-items: center;
		padding: 0 4rem;
	}

	.subheading {
		width: fit-content;
		margin: 2.5rem 0 1rem;
		font-size: 2rem;
		font-weight: 500;
	}

	.voki-type-container {
		display: flex;
		flex-direction: row;
		gap: 4rem;
	}

	.name-input {
		width: 100%;
		height: 2.5rem;
		padding: 0.125rem 0.5rem;
		border: none;
		border: 0.2rem solid transparent;
		border-radius: 0.5rem;
		background-color: var(--secondary);
		color: var(--text);
		font-size: 1.5rem;
		font-weight: 420;
		letter-spacing: 0.25px;
		box-shadow: var(--shadow);
		transition: background-color 0.08s ease-in-out;
	}

	.name-input:focus {
		outline: none;
		border-color: var(--primary);
	}

	:global(#voki-initializing-dialog .err-block) {
		margin: 1rem;
	}

	:global(#voki-initializing-dialog .primary-btn) {
		padding: 0.25rem 1.5rem;
		margin: auto auto 2rem;
	}
</style>
