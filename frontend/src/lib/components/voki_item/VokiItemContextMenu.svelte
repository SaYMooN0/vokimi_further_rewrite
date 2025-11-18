<script lang="ts">
	import BaseContextMenu from "../BaseContextMenu.svelte";


	let container: BaseContextMenu = $state<BaseContextMenu>()!;

	export function open(x: number, y: number, vokiId: string) {
		container.open(x, y);
	}
</script>

<BaseContextMenu bind:this={container}>
	<div>Content</div>
</BaseContextMenu>
