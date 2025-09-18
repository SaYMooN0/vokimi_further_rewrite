<script lang="ts">
	import { toast } from 'svelte-sonner';
	interface Props {
		icon: string;
		mainColor: string;
		secondColor: string;
	}
	let { icon = $bindable(), mainColor, secondColor }: Props = $props();
	const allIconIds = [
		'albums-bookmark-1-icon',
		'albums-bookmark-2-icon',
		'albums-clock-1-icon',
		'albums-clock-2-icon',
		'albums-star-1-icon',
		'albums-star-2-icon'
	];
	function openInput() {
		toast.error("Sorry, this feature isn't implemented yet");
	}
</script>

<div style="

--icon-color-1: {mainColor}; --icon-color-2: {secondColor};" onclick={openInput}>
	<svg><use href="#{icon}"></use></svg>
</div>

<style>

</style>
