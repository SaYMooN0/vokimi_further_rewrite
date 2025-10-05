<script lang="ts">
	interface Props {
		text: string;
		href: string;
		iconId: string;
		iconMainColor: string;
		iconSecondaryColor: string;
	}
	let { text, href, iconId, iconMainColor, iconSecondaryColor }: Props = $props();
</script>

<a
	{href}
	class="album-link"
	style="
--icon-color-1: {iconMainColor}; --icon-color-2: {iconSecondaryColor};"
>
	<svg><use href={iconId} /></svg>
	<label>{text}</label>
</a>

<style>
	.album-link {
		border: 0.25rem solid var(--icon-color-1);
		border-radius: 1.25rem;
		width: 100%;
		min-height: 6rem;
		display: grid;
		grid-template-rows: 1fr auto;
		justify-content: center;
		justify-items: center;
		padding: 0.5rem 0;
		gap: 0.25rem;
		transition: all 0.12s ease-in;
	}
	.album-link svg {
		width: 40%;
		aspect-ratio: 1/1;
	}
	.album-link label {
		font-size: 1.5rem;
		font-weight: 500;
		color: var(--text);
		transition: inherit;
	}
	.album-link:hover {
		transform: scale(1.04);
	}
	.album-link:hover label {
		letter-spacing: 1px;
	}
</style>
