<script lang="ts">
	interface Props {
		viewerId: string;
		primaryAuthorId: string;
		creationDate: Date;
	}
	let { viewerId, primaryAuthorId, creationDate }: Props = $props();
</script>

<div class="primary-author-container" class:viewer-is-primary-author={viewerId === primaryAuthorId}>
	Primary authors
</div>
