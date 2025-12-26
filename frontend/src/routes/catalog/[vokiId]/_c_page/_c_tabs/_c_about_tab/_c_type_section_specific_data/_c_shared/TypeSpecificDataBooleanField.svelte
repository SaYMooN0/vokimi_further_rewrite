<script lang="ts">
	interface Props {
		fieldName: string;
		value: boolean;
	}
	let { fieldName, value }: Props = $props();
</script>

<div class="field-line">
	<div class="type-value">
		<span>{fieldName}</span>
	</div>
	<div class="value">
		{value ? 'Yes' : 'No'}
	</div>
</div>

<style>
	.field-line {
		display: flex;
		flex-direction: row;
		align-items: center;
		margin: 0;
		cursor: default;
	}

	.field-line .type-value {
		display: flex;
		flex-direction: row;
		align-items: center;
		margin-left: 0.5rem;
		font-size: 1.125rem;
		font-weight: 500;
	}
</style>
