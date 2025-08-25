<script lang="ts">
	import FieldNotSetLabel from '$lib/components/FieldNotSetLabel.svelte';
	import { StringUtils } from '$lib/ts/utils/string-utils';

	let { description }: { description: string } = $props<{ description: string }>();
</script>

{#if StringUtils.isNullOrWhiteSpace(description)}
	<FieldNotSetLabel text="No description" />
{:else}
	<p class="description">
		{description}
	</p>
{/if}

<style>
	.description {
		font-size: 1.5rem;
		font-weight: 450;
		margin: 0;
		text-indent: 0.5em;
		word-break: break-all;
	}
</style>
