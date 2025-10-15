<script lang="ts">
	interface Props {
		text: string;
	}
	let { text = $bindable() }: Props = $props();
</script>

<header class="header">
	<h1 class="title">{text}</h1>
	<p class="note">
		<svg><use href="#common-info-icon" /></svg>
		Note: every choice can be changed later
	</p>
</header>

<style>
	.header {
		display: grid;
		gap: 0.25rem;
		padding-bottom: 0.5rem;
	}

	.title {
		font-size: 1.125rem;
		line-height: 1.35;
		font-weight: 700;
		letter-spacing: 0.25px;
		color: var(--text);
	}
	svg {
		height: 1rem;
		width: 1rem;
	}
	.note {
		font-size: 1rem;
		color: var(--muted-foreground);
	}
</style>
