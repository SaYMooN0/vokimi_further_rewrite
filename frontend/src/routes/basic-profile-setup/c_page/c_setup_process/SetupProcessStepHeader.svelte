<script lang="ts">
	interface Props {
		text: string;
	}
	let { text = $bindable() }: Props = $props();
</script>

<header class="header">
	<h1 class="title">{text}</h1>
	<p class="note">
		<span>Note:</span> every choice can be changed later
	</p>
</header>

<style>
	.header {
		display: grid;
		height: 4.5rem;
		gap: 0.5rem;
		align-items: center;
		justify-content: center;
		align-content: center;
	}

	.title {
		font-size: 1.75rem;
		line-height: 1.35;
		font-weight: 700;
		letter-spacing: 0.25px;
		color: var(--text);
	}
	.note {
		text-align: center;
		font-size: 1.125rem;
		color: var(--secondary-foreground);
		font-weight: 450;
	}

	.note span {
		font-weight: 580;
	}
</style>
