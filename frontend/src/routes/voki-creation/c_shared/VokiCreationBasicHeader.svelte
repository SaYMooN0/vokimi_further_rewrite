<script lang="ts">
	const { header, className = '' }: { header: string; className?: string } = $props<{
		header: string;
		className?: string;
	}>();
</script>

<h1 class={className}>{header}</h1>

<style>
	h1 {
		display: block;
		width: 100%;
		margin: 0.75rem 0 1.25rem;
		color: var(--text);
		font-size: 1.75rem;
		font-weight: 550;
		text-align: center;
		letter-spacing: 0.5px;
	}
</style>
