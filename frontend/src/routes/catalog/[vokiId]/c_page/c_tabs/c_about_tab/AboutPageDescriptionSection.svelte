<script lang="ts">
	import FieldNotSetLabel from '$lib/components/FieldNotSetLabel.svelte';
	import { StringUtils } from '$lib/ts/utils/string-utils';
	import AboutPageFieldNameLabel from './AboutPageFieldNameLabel.svelte';

	let { description }: { description: string } = $props<{ description: string }>();
	description =
		'dsadjklas hdas dhjka hdjkash dhajks hdahsjkdhjkah jhdjah hdjksahj dhjs hdjhj dhjkas';
</script>

<p class="description">
	<AboutPageFieldNameLabel fieldName="Description:" />
	{#if StringUtils.isNullOrWhiteSpace(description)}
		<FieldNotSetLabel text="No description" />
	{:else}
		<label class="value">
			{description}
		</label>
	{/if}
</p>

<style>
	.description {
		margin: 0;
	}

	.description > .value {
		font-size: 1.25rem;
		font-weight: 440;
		text-indent: 0.5em;
		word-break: break-all;
	}
</style>
