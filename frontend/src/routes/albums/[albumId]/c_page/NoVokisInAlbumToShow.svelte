<script lang="ts">
	const messages = {
		empty: {
			
		},
		filtered: {}
	};
	interface Props {
		title: string;
		subtitle: string;
	}
	let { title, subtitle }: Props = $props();
</script>

<div class="no-vokis">
	<h2 class="title">{title}</h2>
	<p class="subtitle">{subtitle}</p>
</div>

<style>
	.no-vokis {
		padding: 2rem;
		text-align: center;
		color: var(--muted-foreground);
        margin: 4rem auto 4rem;
	}

	.title {
		font-size: 1.5rem;
		font-weight: 600;
		margin-bottom: 0.25rem;
		color: var(--text);
	}

	.subtitle {
		font-size: 1.125em;
		color: var(--muted-foreground);
	}
</style>
