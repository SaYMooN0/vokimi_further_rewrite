<script lang="ts">
	const { fieldName, className = '' }: { fieldName: string; className?: string } = $props<{
		fieldName: string;
		className?: string;
	}>();
</script>

<label class={className}>{fieldName}</label>

<style>
	label {
		font-size: 1.25rem;
		font-weight: 440;
		color: var(--secondary-foreground);
		text-decoration: underline;
		margin-right: 0.25rem;
		text-underline-offset: 0.125rem;
	}
</style>
