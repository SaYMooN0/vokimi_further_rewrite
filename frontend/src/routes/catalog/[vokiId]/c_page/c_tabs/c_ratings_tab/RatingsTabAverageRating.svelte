<script lang="ts">
	interface Props {
		averageRating: number;
		count: number;
	}
	let { averageRating, count }: Props = $props();

	const avgPct = $derived(Math.max(0, Math.min(100, (averageRating / 5) * 100)));
</script>

<div class="avg-card">
	{averageRating}
</div>
