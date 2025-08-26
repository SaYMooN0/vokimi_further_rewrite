<script lang="ts">
	let { name }: { name: string } = $props<{ name: string }>();
</script>

<h1 class="voki-name">{name}</h1>

<style>
	.voki-name {
		margin: 0;
		font-size: 2.25rem;
		font-weight: 500;
		text-indent: 0.5em;
		word-break: break-all;
	}
</style>
