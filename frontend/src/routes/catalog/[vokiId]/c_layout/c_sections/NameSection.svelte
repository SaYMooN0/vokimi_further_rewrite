<script lang="ts">
	let { name }: { name: string } = $props<{ name: string }>();
</script>

<h1 class="voki-name">{name}</h1>

<style>
	.voki-name {
		font-size: 2rem;
		font-weight: 500;
		margin: 0;
		text-indent: 0.5em;
		word-break: break-all;

	}
</style>
