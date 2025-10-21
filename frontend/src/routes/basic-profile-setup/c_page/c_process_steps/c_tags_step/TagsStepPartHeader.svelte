<script lang="ts">
	let { text }: { text: string } = $props<{ text: string }>();
</script>

<h1 class="part-header">{text}</h1>

<style>
	.part-header {
		font-size: 1.25rem;
		color: var(--text);
		font-weight: 500;
		margin: 0 0 0.5rem 0;
		text-align: center;
	}
</style>
