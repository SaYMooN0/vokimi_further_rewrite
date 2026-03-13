<script lang="ts">
	interface Props {
		title: string;
	}

	let { title }: Props = $props();
</script>

<h2 class="profile-sidebar-subheader">{title}</h2>

<style>
	.profile-sidebar-subheader {
		font-size: 1.25rem;
		font-weight: 700;
		margin: 0;
	}
</style>
