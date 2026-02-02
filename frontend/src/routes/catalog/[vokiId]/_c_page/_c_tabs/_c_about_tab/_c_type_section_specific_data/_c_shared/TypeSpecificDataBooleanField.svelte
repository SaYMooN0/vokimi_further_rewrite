<script lang="ts">
	interface Props {
		fieldName: string;
		value: boolean;
		trueLabel?: string;
		falseLabel?: string;
	}
	let { fieldName, value, trueLabel = 'Yes', falseLabel = 'No' }: Props = $props();
</script>

<div class="field-line">
	<div class="name">
		<span>{fieldName}</span>
	</div>
	<div class="value">
		{value ? trueLabel : falseLabel}
	</div>
</div>

<style>
	.field-line {
		display: flex;
		flex-direction: row;
		align-items: center;
		cursor: default;
	}

	.name {
		display: flex;
		flex-direction: row;
		align-items: center;
		font-size: 1.125rem;
		font-weight: 425;
		color: var(--muted-foreground);
	}

	.value {
		font-size: 1.125rem;
		font-weight: 425;
		margin-left: 0.5rem;
		font-weight: 450;
	}
</style>
