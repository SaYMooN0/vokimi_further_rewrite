<script lang="ts">
	import { navigating, page } from '$app/state';
	import type { Snippet } from 'svelte';
	import GeneralVokiCreationLayoutNavBar from './c_layout/GeneralVokiCreationLayoutNavBar.svelte';
	import { type VokiCreationHeaderVokiName } from '../../c_layout/VokiCreationVokiNameHeader.svelte';
	import { setVokiCreationPageContext } from '../../voki-creation-page-context';
	import CubesLoader from '$lib/components/loaders/CubesLoader.svelte';
	import vokiAnswerTypesIconsSprite from '$lib/icons/general-voki-answer-types-icons.svg?raw';
	import generalVokiCreationIconsSprite from '$lib/icons/general-voki-creation-icons.svg?raw';
	import { ApiVokiCreationGeneral } from '$lib/ts/backend-communication/voki-creation-backend-service';
	import VokiCreationVokiNameHeader from '../../c_layout/VokiCreationVokiNameHeader.svelte';

	const { children }: { children: Snippet } = $props();

	let vokiName: VokiCreationHeaderVokiName = $state()!;
	async function fetchAndSetVokiName() {
		if (!page.params.vokiId) {
			vokiName = {
				state: 'errs',
				errs: [{ message: 'Voki id is not specified' }],
				reload: () => fetchAndSetVokiName()
			};
			return;
		}
		vokiName = { state: 'loading' };
		const response = await ApiVokiCreationGeneral.getVokiName(page.params.vokiId);
		if (response.isSuccess) {
			vokiName = { state: 'ok', value: response.data.vokiName };
		} else {
			vokiName = {
				state: 'errs',
				errs: response.errs,
				reload: () => fetchAndSetVokiName()
			};
		}
	}
	fetchAndSetVokiName();
	let headerVokiName = $derived({
		value: vokiName.state === 'ok' ? vokiName.value : undefined,
		invalidate: () => fetchAndSetVokiName()
	});
	setVokiCreationPageContext(ApiVokiCreationGeneral, headerVokiName);
</script>

<div class="sprites">
	{@html vokiAnswerTypesIconsSprite}
	{@html generalVokiCreationIconsSprite}
</div>

<VokiCreationVokiNameHeader {vokiName} vokiType="General" />
<GeneralVokiCreationLayoutNavBar />

{#if navigating.type}
	<div class="loading fade-in-animation">
		<h1>Loading tab data</h1>
		<CubesLoader sizeRem={5} color="var(--primary)" />
	</div>
{:else}
	<div class="fade-in-animation">
		{@render children()}
	</div>
{/if}

<style>
	.sprites {
		display: none;
		width: 0;
		height: 0;
	}

	.loading {
		display: flex;
		flex-direction: column;
		align-items: center;
		gap: 2rem;
		margin-top: 12rem;
	}

	.loading h1 {
		color: var(--secondary-foreground);
		font-size: 2.5rem;
		font-weight: 600;
		letter-spacing: 2px;
	}

	.fade-in-animation {
		animation: fade-in 0.06s ease-in-out forwards;
	}

	@keyframes fade-in {
		from {
			opacity: 0.4;
		}

		to {
			opacity: 1;
		}
	}
</style>
