<script lang="ts">
	interface Props {
		addNewAnswer: () => void;
	}
	let { addNewAnswer }: Props = $props();
</script>

<p>This question has no answers. Add one to get started</p>
<button onclick={addNewAnswer}>Add answer</button>
