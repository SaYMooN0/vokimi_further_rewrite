<script lang="ts">
	import type { VokiType } from '$lib/ts/voki-type';
	import GoToVokiCreationLink from '../../_c_shared/GoToVokiCreationLink.svelte';

	interface Props {
		id: string;
		type: VokiType;
		name: string;
	}

	let { id, type, name }: Props = $props();
</script>

<div class="voki-initialized-container">
	<p class="initialized-msg">
		New {type} <span class="name">{name}</span> voki has been initialized
	</p>
	<GoToVokiCreationLink vokiId={id} vokiType={type} />
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
