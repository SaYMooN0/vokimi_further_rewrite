<script lang="ts">
	import type { VokiItemViewErrStateProps, VokiItemViewOkStateProps } from './c_voki_item/types';
	import VokiItemViewErrState from './c_voki_item/VokiItemViewErrState.svelte';
	import VokiItemViewLoadingState from './c_voki_item/VokiItemViewLoadingState.svelte';
	import VokiItemViewOkState from './c_voki_item/VokiItemViewOkState.svelte';

	export type VokiItemViewState =
		| { name: 'ok'; data: VokiItemViewOkStateProps }
		| { name: 'loading' }
		| { name: 'errs'; data: VokiItemViewErrStateProps };

	interface Props {
		state: VokiItemViewState;
	}

	let { state }: Props = $props();
</script>

{#if state.name === 'ok'}
	<VokiItemViewOkState {...state.data} />
{:else if state.name === 'loading'}
	<VokiItemViewLoadingState />
{:else if state.name === 'errs'}
	<VokiItemViewErrState {...state.data} />
{/if}
