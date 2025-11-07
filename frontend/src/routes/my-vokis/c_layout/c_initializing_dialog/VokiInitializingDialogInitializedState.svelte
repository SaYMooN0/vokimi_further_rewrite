<script lang="ts">
	import { goto } from '$app/navigation';
	import PrimaryButton from '$lib/components/buttons/PrimaryButton.svelte';
	import { StringUtils } from '$lib/ts/utils/string-utils';
	import type { VokiType } from '$lib/ts/voki-type';

	interface Props {
		id: string;
		type: VokiType;
		name: string;
	}

	let { id, type, name }: Props = $props();
	function goToVokiPage() {
		goto(`/voki-creation/${StringUtils.pascalToKebab(type)}/${id}`);
	}
</script>

<div class="voki-initialized-container">
	<p class="initialized-msg">
		New {type} <span class="name">{name}</span> voki has been initialized
	</p>
	<PrimaryButton onclick={() => goToVokiPage()}>Go to Voki creation</PrimaryButton>
</div>

<style>
	.voki-initialized-container {
		display: flex;
		flex-direction: column;
		align-items: center;
		gap: 2rem;
		width: 100%;
		padding: 1rem 2rem;
		margin: auto 0;
	}

	.initialized-msg {
		margin: 0;
		color: var(--muted-foreground);
		font-size: 2rem;
		font-weight: 500;
		text-align: center;
	}

	.name {
		color: var(--primary);
		font-weight: 600;
	}
</style>
