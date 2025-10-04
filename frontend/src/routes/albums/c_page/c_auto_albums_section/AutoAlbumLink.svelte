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
	style="--icon-color-1: {iconMainColor}; --icon-color-2: {iconSecondaryColor};"
>
	<svg><use href={iconId} /></svg>
	<label>{text}</label>
</a>
