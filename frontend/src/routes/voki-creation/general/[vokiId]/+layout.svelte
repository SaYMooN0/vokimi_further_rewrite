<script lang="ts">
	import { navigating } from '$app/state';
	import type { Snippet } from 'svelte';
	import GeneralVokiCreationLayoutNavBar from './c_layout/GeneralVokiCreationLayoutNavBar.svelte';
	import VokiCreationHeader from '../../c_layout/VokiCreationHeader.svelte';
	import { setVokiCreationPageApiService } from '../../voki-creation-page-context';
	import CubesLoader from '$lib/components/loaders/CubesLoader.svelte';
	import vokiAnswerTypesIconsSprite from '$lib/icons/general-voki-answer-types-icons.svg?raw';
	import generalVokiCreationIconsSprite from '$lib/icons/general-voki-creation-icons.svg?raw';
	import imageIconsSprite from '$lib/icons/image-icons.svg?raw';
	const { children }: { children: Snippet } = $props();
	setVokiCreationPageApiService('General');
</script>

<div class="sprites">
	{@html vokiAnswerTypesIconsSprite}
	{@html generalVokiCreationIconsSprite}
	{@html imageIconsSprite}
</div>

<VokiCreationHeader vokiName="Voki name bla bla ki name ki name " typeName="general" />
<GeneralVokiCreationLayoutNavBar />

{#if navigating.type}
	<div class="loading fade-in-animation">
		<h1>Loading tab data</h1>
		<CubesLoader sizeRem={5} />
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
