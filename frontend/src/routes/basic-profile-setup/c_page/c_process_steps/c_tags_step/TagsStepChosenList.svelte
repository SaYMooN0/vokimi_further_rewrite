<script lang="ts">
	interface Props {
		chosenTags: Set<string>;
		removeTag: (tag: string) => void;
	}
	let { chosenTags, removeTag }: Props = $props();
</script>


	<div class="chosen-tags-list">
		{#each chosenTags as tag}
			<div class="tag">
				<span>#{tag}</span>
				<svg onclick={() => removeTag(tag)}><use href="#common-cross-icon" /></svg>
			</div>
		{/each}
	</div>

<style>

	.chosen-tags-list {
		display: flex;
		justify-content: center;
		column-gap: 0.5rem;
		row-gap: 1rem;
		flex-wrap: wrap;
		align-items: center;
		height: fit-content;
	}

	.tag {
		background-color: var(--secondary);
		color: var(--secondary-foreground);
		display: grid;
		grid-template-columns: 1fr auto;
		gap: 0.125rem;
		align-items: center;
		padding: 0.125rem 0.675rem;
		border-radius: 1rem;
		cursor: default;
		height: fit-content;
	}
	.tag span {
		font-size: 1.125rem;
		letter-spacing: 0.1px;
		font-weight: 450;
	}
	.tag svg {
		width: 1.125rem;
		height: 1.125rem;
		cursor: pointer;
		stroke-width: 2;
		color: var(--secondary-foreground);
	}
	.tag svg:hover {
		color: var(--muted-foreground);
		stroke-width: 2.5;
	}
	.tag:has(> svg:hover) > span {
		animation: shake 1s ease-in-out infinite;
        animation-delay: 0.5s;
	}
	@keyframes shake {
		0%,
		100% {
			transform: scale(1);
		}
		25% {
			transform: scale(0.98);
		}
        50% {
            transform: scale(1);
        }
        75% {
            transform: scale(0.965);
        }
	}
</style>
