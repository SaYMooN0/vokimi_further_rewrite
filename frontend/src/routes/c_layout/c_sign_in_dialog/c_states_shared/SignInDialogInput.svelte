<script lang="ts">
	import { StringUtils } from '$lib/ts/utils/string-utils';
	import type { Snippet } from 'svelte';
	interface Props {
		type?: 'text' | 'password' | 'email';
		fieldName: string;
		value: string;
		children: Snippet;
	}
	let { type = 'text', fieldName, value = $bindable(), children }: Props = $props();
	let inputName = StringUtils.rndStr(8);
</script>

<div class="input-field unselectable">
	<input bind:value {type} name={inputName} required autocomplete="off" placeholder=" " />
	<label for={inputName}>
		{@render children()}
		{fieldName}
	</label>
</div>

<style>
	.input-field {
		position: relative;
		width: 100%;
		margin: 0.75rem 0 0.5rem;
	}

	.input-field label {
		position: absolute;
		top: 50%;
		left: 0.75rem;
		display: flex;
		flex-direction: row;
		align-items: center;
		gap: 0.25rem;
		padding: 0 0.125rem;
		border: none;
		border-radius: 100rem;
		background-color: transparent;
		background-color: var(--back);
		color: var(--secondary-foreground);
		font-size: 1.125rem;
		transition: all 0.3s ease;
		transform: translateY(-50%);
		pointer-events: none;
	}

	

	.input-field input {
		width: 100%;
		box-sizing: border-box;
		padding: 0.625rem 1rem;
		border: solid 0.125rem var(--secondary-foreground);
		border-radius: 8px;
		background-color: var(--back);
		font-size: 1.125rem;
		letter-spacing: 1px;
		outline: none;
		box-shadow: var(--shadow-md);

	}

	.input-field input:focus,
	.input-field input:not(:placeholder-shown) {
		border-color: var(--primary);
	}

	.input-field input:focus ~ label,
	.input-field input:not(:placeholder-shown) ~ label {
		top: 0;
		color: var(--primary);
		transform: translateY(-50%) scale(0.95);
	}

	:global(.input-field label > svg) {
		height: 1.375rem;
		color: inherit;
		transition: inherit;
	}
</style>
