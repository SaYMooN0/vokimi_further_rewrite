<script lang="ts">
	import type { VokiType } from '$lib/ts/voki-type';
	import GoToVokiCreationLink from '../../../_c_shared/GoToVokiCreationLink.svelte';

	interface Props {
		id: string;
		type: VokiType;
		name: string;
	}

	let { id, type, name }: Props = $props();
</script>

<div class="voki-initialized-container">
	<p class="initialized-msg">
		New {type} <span class="name">{name}</span> Voki has been initialized
	</p>
	<GoToVokiCreationLink vokiId={id} vokiType={type} />
</div>

<style>
	.voki-initialized-container {
		height: 100%;
		display: flex;
		flex-direction: column;
		align-items: center;
		gap: 2.5rem;
		width: 100%;
		margin: 0 auto;
		text-align: center;
		overflow-y: auto;
	}

	.initialized-msg {
		margin: 0;
		color: var(--text);
		font-size: 2.125rem;
		font-weight: 700;
		text-align: center;
		letter-spacing: -0.025em;
		line-height: 1.25;
		text-wrap: balance;
		overflow-wrap: anywhere;
		word-break: break-word;
		max-width: 40rem;
	}

	.name {
		color: var(--primary);
		overflow-wrap: anywhere;
		display: -webkit-box;
		-webkit-line-clamp: 4;
		line-clamp: 4;
		-webkit-box-orient: vertical;
		overflow: hidden;
		text-overflow: ellipsis;
	}
</style>
