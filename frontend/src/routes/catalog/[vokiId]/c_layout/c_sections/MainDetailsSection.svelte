<script lang="ts">
	import type { Language } from "$lib/ts/language";

	const {
		type,
		language,
		isAgeRestricted
	}: { type: string; language: Language; isAgeRestricted: boolean } = $props<{
		type: string;
		language: Language;
		isAgeRestricted: boolean;
	}>();
</script>

<div class="main-details-section">
	<label>{type}</label>
	<div class="separator"></div>
	<label>{language}</label>
	<div class="separator"></div>

	<label>{isAgeRestricted ? 'Age restricted' : 'No age restriction'}</label>
</div>

<style>
	.main-details-section {
		display: grid;
		grid-template-columns: 1fr auto 1fr auto 1fr;
	}
	.separator {
		width: 1px;
		height: 100%;
		background-color: var(--secondary-foreground);
	}
</style>
