<script lang="ts">
	const { fieldName, className = '' }: { fieldName: string; className?: string } = $props<{
		fieldName: string;
		className?: string;
	}>();
</script>

<label class={className}>{fieldName}</label>

<style>
	label {
		margin: 0 0.375rem;
		color: var(--secondary-foreground);
		font-size: 1.125rem;
		font-weight: 450;
		letter-spacing: -0.25px;
	}
</style>
