<script lang="ts">
	interface Props {
		displayName: string;
	}
	let { displayName = $bindable() }: Props = $props();
</script>

<h1>display name</h1>
