<script lang="ts">
	interface Props {
		openInviteDialog: () => void;
	}
	let { openInviteDialog }: Props = $props();
</script>

<div class="invite-coauthor-card">
	<svg class="group-icon" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" fill="none">
		<path
			d="M15 8C15 9.65685 13.6569 11 12 11C10.3431 11 9 9.65685 9 8C9 6.34315 10.3431 5 12 5C13.6569 5 15 6.34315 15 8Z"
			stroke="currentColor"
			stroke-linecap="round"
			stroke-linejoin="round"
		></path>
		<path
			d="M16 4C17.6568 4 19 5.34315 19 7C19 8.22309 18.268 9.27523 17.2183 9.7423"
			stroke="currentColor"
			stroke-linecap="round"
			stroke-linejoin="round"
		></path>
		<path
			d="M13.7143 14H10.2857C7.91876 14 5.99998 15.9188 5.99998 18.2857C5.99998 19.2325 6.76749 20 7.71426 20H16.2857C17.2325 20 18 19.2325 18 18.2857C18 15.9188 16.0812 14 13.7143 14Z"
			stroke="currentColor"
			stroke-linecap="round"
			stroke-linejoin="round"
		></path>
		<path
			d="M17.7143 13C20.0812 13 22 14.9188 22 17.2857C22 18.2325 21.2325 19 20.2857 19"
			stroke="currentColor"
			stroke-linecap="round"
			stroke-linejoin="round"
		></path>
		<path
			d="M8 4C6.34315 4 5 5.34315 5 7C5 8.22309 5.73193 9.27523 6.78168 9.7423"
			stroke="currentColor"
			stroke-linecap="round"
			stroke-linejoin="round"
		></path>
		<path
			d="M3.71429 19C2.76751 19 2 18.2325 2 17.2857C2 14.9188 3.91878 13 6.28571 13"
			stroke="currentColor"
			stroke-linecap="round"
			stroke-linejoin="round"
		></path>
	</svg>
	<h2 class="title">No co-authors yet</h2>
	<p class="subtitle">
		This Voki doesn’t have any co-authors<br />Invite the first one to start collaborating!
	</p>
	<button class="invite-btn" onclick={openInviteDialog}>Invite first co-author</button>
</div>

<style>
	.invite-coauthor-card {
		margin-top: 1rem;
		display: flex;
		flex-direction: column;
		align-items: center;
		justify-content: center;
		text-align: center;
		border-radius: 1rem;
		background-color: var(--back);
		width: 100%;
		animation: var(--appear-animation) 11s ease-in;
	}

	.group-icon {
		height: 4rem;
		width: 4rem;
		line-height: 1;
		stroke-width: 1.5;
	}

	.title {
		font-size: 1.375rem;
		font-weight: 600;
		color: var(--text);
		margin: -0.25rem 0 0.25rem;
	}

	.subtitle {
		font-size: 0.9375rem;
		color: var(--muted-foreground);
		max-width: 26rem;
		line-height: 1.5;
	}

	.invite-btn {
		margin-top: 1rem;
		font-size: 1rem;
		font-weight: 500;
		color: var(--primary-foreground);
		background-color: var(--primary);
		border: none;
		border-radius: 10rem;
		padding: 0.5rem 1.5rem;
		letter-spacing: 0.4px;
		outline: none;
		cursor: pointer;
		transition: background-color 0.15s ease;
		display: flex;
		align-items: center;
		gap: 0.375rem;
		transition: padding 0.15s ease;
	}
	.invite-btn:hover {
		background-color: var(--primary-hov);
		padding: 0.5rem 1.75rem;
	}

	@keyframes --appear-animation {
		from {
			opacity: 0;
			transform: scale(0.6);
		}

		to {
			opacity: 1;
			transform: scale(1);
		}
	}
</style>
