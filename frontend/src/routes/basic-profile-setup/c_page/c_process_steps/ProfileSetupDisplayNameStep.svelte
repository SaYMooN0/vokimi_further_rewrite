<script lang="ts">
	interface Props {
		displayName: string;
		maxLength: number;
	}
	let { displayName = $bindable(), maxLength = 30 }: Props = $props();
</script>

<div class="display-name-container">
	<span class="at-symbol">@</span>
	<input
		type="text"
		class="name-input"
		bind:value={displayName}
		style="width: {maxLength}ch"
		maxlength={maxLength}
	/>
	<div class="len-count">
		<span class="current" class:err={displayName.length > maxLength}>{displayName.length}</span>
		<span class="max">/{maxLength}</span>
	</div>
</div>

<style>
	.display-name-container {
		display: grid;
		width: fit-content;
		margin-inline: auto;
		position: relative;
	}
	.at-symbol {
		position: absolute;
		top: 50%;
		padding: 0 0.75rem;
		transform: translateY(-50%);
		font-size: 1.5rem;
		font-weight: 600;
		color: var(--secondary-foreground);
		pointer-events: none;
	}

	.name-input {
		--border-color: var(--secondary-foreground);
		max-width: calc(var(--width-limit) - var(--sidebar-width) - 8rem);
		font-size: 1.25rem;
		font-weight: 450;
		padding: 0.675rem 1rem 0.675rem 2.25rem;
		border-radius: 0.5rem;
		border: 0.125rem solid var(--border-color);
		background-color: var(--back);
		color: var(--text);
		outline: none;
		letter-spacing: 1px;
	}
	.name-input:hover,
	.name-input:focus {
		--border-color: var(--primary);
	}

	.display-name-container:has(input:focus) .at-symbol {
		color: var(--primary);
		font-weight: 700;
	}
	.len-count {
		position: absolute;
		font-size: 0.875rem;
		right: 1rem;
		top: calc(100%);
		transform: translateY(-50%);
		background-color: var(--back);
		display: grid;
		padding: 0 0.125rem;
		grid-template-columns: auto auto;
		font-weight: 450;
		pointer-events: none;
		font-variant-numeric: tabular-nums;
	}
	.len-count .current {
		text-align: right;
	}
	.len-count .current.err {
		color: var(--err-foreground);
		font-weight: 500;
	}
	.len-count .max {
		text-align: left;
	}
</style>
