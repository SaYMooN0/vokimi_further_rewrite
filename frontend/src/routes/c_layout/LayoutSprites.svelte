<script lang="ts">
	import vokiTypesIconsSprite from '$lib/icons/voki-type-icons.svg?raw';
	import commonIconsSprite from '$lib/icons/common-icons.svg?raw';
	import caretIconsSprite from '$lib/icons/caret-icons.svg?raw';
	import errorIconsSprite from '$lib/icons/error-icons.svg?raw';
	import languagesIconsSprite from '$lib/icons/languages-icons.svg?raw';
	import albumIconsSprite from '$lib/icons/album-icons.svg?raw';
	import imageIconsSprite from '$lib/icons/image-icons.svg?raw';
</script>

<div class="sprites">
	{@html vokiTypesIconsSprite}
	{@html commonIconsSprite}
	{@html caretIconsSprite}
	{@html errorIconsSprite}
	{@html languagesIconsSprite}
	{@html albumIconsSprite}
	{@html imageIconsSprite}
</div>

<style>
	.sprites {
		height: 0;
	}

	.sprites > :global(svg) {
		width: 0;
		height: 0;
	}
</style>
