<script lang="ts">
	interface Props {
		profilePic: string;
	}
	let { profilePic = $bindable() }: Props = $props();
</script>

<h1>display name</h1>
