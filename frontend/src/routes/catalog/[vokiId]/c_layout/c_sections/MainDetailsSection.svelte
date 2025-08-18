<script lang="ts">
	import { LanguageUtils, type Language } from '$lib/ts/language';
	import { StringUtils } from '$lib/ts/utils/string-utils';

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
	<div class="value-container type-value">
		<svg><use href="#{StringUtils.pascalToKebab(type)}-voki-type-icon" /></svg>
		<label>{type}</label>
	</div>
	<div class="separator"></div>
	<div class="value-container language-value">
		<svg><use href="#languages-icons-{StringUtils.pascalToKebab(language)}" /></svg>
		<label>{LanguageUtils.name(language)}</label>
	</div>
	<div class="separator"></div>
	<label>{isAgeRestricted ? 'Age restricted' : 'No age restriction'}</label>
</div>

<style>
	.main-details-section {
		display: grid;
		grid-template-columns: 1fr auto 1fr auto 1fr;
		justify-content: center;
		align-items: center;
		justify-items: center;
	}

	.separator {
		width: 0;
		box-shadow: var(--shadow-xs);
		height: 100%;
		background-color: var(--muted);
	}
	.value-container {
		display: flex;
		flex-direction: row;
		justify-content: center;
		align-items: center;
		gap: 0.5rem;
	}
	.main-details-section label {
		font-size: 1.25rem;
		font-weight: 450;
	}
	.type-value > svg {
		height: 1.75rem;
		width: 1.75rem;
		color: var(--primary);
		stroke-width: 1.8;
	}
	.language-value > svg {
		height: 1.5rem;
		aspect-ratio: var(--lang-icon-aspect-ratio);
		border-radius: 0.375rem;
	}
</style>
