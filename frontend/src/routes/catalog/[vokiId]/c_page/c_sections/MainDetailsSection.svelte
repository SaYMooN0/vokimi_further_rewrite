<script lang="ts">
	import { LanguageUtils, type Language } from '$lib/ts/language';
	import { StringUtils } from '$lib/ts/utils/string-utils';
	import type { VokiType } from '$lib/ts/voki-type';

	interface Props {
		type: VokiType;
		language: Language;
		hasMatureContent: boolean;
	}
	const { type, language, hasMatureContent }: Props = $props();
</script>

<div class="main-details-section">
	<div class="value-container type-value">
		<label class="value-name">Type:</label>
		<svg><use href="#{StringUtils.pascalToKebab(type)}-voki-type-icon" /></svg>
		<label class="value-label">{type}</label>
	</div>
	<div class="separator"></div>
	<div class="value-container language-value">
		<label class="value-name">Language:</label>
		<svg><use href={LanguageUtils.icon(language)} /></svg>	
		<label class="value-label">{LanguageUtils.name(language)}</label>
	</div>
	<div class="separator"></div>
	<label class="value-label">{hasMatureContent ? 'Mature content' : 'No mature content'}</label>
</div>

<style>
	.main-details-section {
		display: grid;
		grid-template-columns: 1fr auto 1fr auto 1fr;
		justify-content: center;
		place-items: center center;
	}

	.separator {
		width: 1px;
		height: 100%;
		background-color: var(--muted);
	}

	.value-container {
		display: flex;
		flex-direction: row;
		justify-content: center;
		align-items: center;
		gap: 0.25rem;
	}

	.value-name {
		margin-right: 0.675rem;
		color: var(--secondary-foreground);
		font-size: 1.15rem;
		font-weight: 450;
	}

	.value-label {
		font-size: 1.25rem;
		font-weight: 450;
	}

	.type-value > svg {
		width: 1.75rem;
		height: 1.75rem;
		color: var(--primary);
		stroke-width: 1.8;
	}

	.language-value > svg {
		height: 1.5rem;
		aspect-ratio: var(--lang-icon-aspect-ratio);
		border-radius: 0.375rem;
		stroke-width: 1.9;
	}
</style>
