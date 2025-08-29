<script lang="ts">
	import { goto } from '$app/navigation';
	import PrimaryButton from '$lib/components/buttons/PrimaryButton.svelte';
	import { StringUtils } from '$lib/ts/utils/string-utils';
	import type { VokiType } from '$lib/ts/voki';

	let { id, type, name }: { id: string; type: VokiType; name: string } = $props<{
		id: string;
		type: VokiType;
		name: string;
	}>();
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
		margin: auto 0;
		display: flex;
		flex-direction: column;
		width: 100%;
		align-items: center;
		gap: 2rem;
		padding: 1rem 2rem;
	}
	.initialized-msg {
		margin: 0;
		color: var(--muted-foreground);
		font-size: 2rem;
		font-weight: 500;
		text-align: center;
	}
	.name {
		font-weight: 600;
		color: var(--primary);
	}
</style>
