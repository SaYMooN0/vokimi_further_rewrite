<script lang="ts">
	import { VokiTypeUtils } from '$lib/ts/voki-type';
	import type { VokiTypeWithSpecificData } from '../../../types';
	import VokiPageTabSectionLabel from '../c_tabs_shared/VokiPageTabSectionLabel.svelte';
	interface Props {
		typeWithData: VokiTypeWithSpecificData;
	}
	let { typeWithData }: Props = $props();
	let showTypeSpecificData = $state(false);
</script>

<div class="container">
	<div class="field-line">
		<VokiPageTabSectionLabel fieldName="Type:" />
		<div class="type-value">
			<span>{VokiTypeUtils.name(typeWithData.type)}</span>
		</div>
	</div>
	<div class="type-specific-data" class:show={showTypeSpecificData}></div>
</div>

<style>
	.container {
		display: flex;
		flex-direction: column;
	}
	.field-line {
		margin: 0;
        		display: flex;
		flex-direction: row;
	}

	.field-line .type-value {
		margin-left: 0.5rem;
		font-size: 1.125rem;
		font-weight: 500;
		display: flex;
		flex-direction: row;
		align-items: center;
	}
</style>
