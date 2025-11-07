<script lang="ts">
	import PrimaryButton from '$lib/components/buttons/PrimaryButton.svelte';
	import DefaultErrBlock from '$lib/components/errs/DefaultErrBlock.svelte';
	import CubesLoader from '$lib/components/loaders/CubesLoader.svelte';
	import { ApiVokiCreationCore, RJO } from '$lib/ts/backend-communication/backend-services';
	import type { Err } from '$lib/ts/err';
	import { StringUtils } from '$lib/ts/utils/string-utils';
	import type { VokiType } from '$lib/ts/voki-type';
	import VokiTypeCard from './VokiTypeCard.svelte';

	interface Props {
		onVokiInitializedSuccessfully: (newVokiData: {
			id: string;
			name: string;
			type: VokiType;
		}) => void;
	}
	let { onVokiInitializedSuccessfully }: Props = $props();

	let selectedVokiType = $state<VokiType>('General');
	let vokiName = $state('');
	let errs: Err[] = $state([]);
	let isLoading = $state(false);

	async function onSubmitBtnClicked() {
		validateForm();
		if (errs.length > 0) {
			return;
		}
		isLoading = true;
		const response = await ApiVokiCreationCore.fetchJsonResponse<{
			id: string;
			name: string;
			type: VokiType;
		}>('/initialize-new-voki', RJO.POST({ newVokiName: vokiName, vokiType: selectedVokiType }));
		if (response.isSuccess) {
			onVokiInitializedSuccessfully(response.data);
		} else {
			isLoading = false;
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

{#if isLoading}
	<div class="loading-container">
		<CubesLoader sizeRem={7} />
		<p>Initializing new voki</p>
	</div>
{:else}
	<p class="subheading">Choose voki type</p>
	<div class="voki-type-container">
		<VokiTypeCard
			type="General"
			isSelected={selectedVokiType === 'General'}
			onclick={() => (selectedVokiType = 'General')}
		/>
		<VokiTypeCard
			type="TierList"
			isSelected={selectedVokiType === 'TierList'}
			onclick={() => (selectedVokiType = 'TierList')}
		/>
		<VokiTypeCard
			type="Scoring"
			isSelected={selectedVokiType === 'Scoring'}
			onclick={() => (selectedVokiType = 'Scoring')}
		/>
	</div>
	<p class="subheading">Input Voki name</p>
	<input bind:value={vokiName} class="name-input" type="text" placeholder="Voki name..." />
	<DefaultErrBlock errList={errs} />
	<PrimaryButton onclick={() => onSubmitBtnClicked()} class="initialize-voki-btn"
		>Create</PrimaryButton
	>
{/if}

<style>
	.loading-container {
		display: flex;
		flex-direction: column;
		align-items: center;
		gap: 2rem;
		width: 100%;
		margin: auto 0;
	}

	.loading-container p {
		margin: 0;
		color: var(--secondary-foreground);
		font-size: 2.5rem;
		font-weight: 600;
		letter-spacing: 0.25px;
	}

	.subheading {
		width: fit-content;
		margin: 0 0 1rem;
		font-size: 2rem;
		font-weight: 500;
	}

	.voki-type-container {
		display: flex;
		flex-direction: row;
		gap: 4rem;
		margin-bottom: 4rem;
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
		font-weight: 450;
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

	:global(.primary-btn.initialize-voki-btn) {
		padding: 0.25rem 1.5rem;
		margin: auto auto 0;
	}
</style>
