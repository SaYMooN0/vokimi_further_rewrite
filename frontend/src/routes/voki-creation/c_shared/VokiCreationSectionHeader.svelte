<script lang="ts">
	const { header, className = '' }: { header: string; className?: string } = $props<{
		header: string;
		className?: string;
	}>();
</script>

<h1 class={className}>{header}</h1>

<style>
	h1 {
		width: 100%;
		color: var(--text);
		text-align: center;
		font-size: 1.5rem;
		font-weight: 450;
	}
</style>
