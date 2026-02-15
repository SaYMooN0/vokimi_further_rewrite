<script lang="ts">
	import { relativeTime } from 'svelte-relative-time';
	import type { ExistingUnfinishedSessionForVokiData } from '$lib/ts/voki-taking-session';
	import { goto } from '$app/navigation';
	import { VokiTakingSessionMarkerCookie } from '$lib/ts/cookies/voki-taking-session-marker';
	import PrimaryButton from '$lib/components/buttons/PrimaryButton.svelte';

	interface Props {
		sessionData: ExistingUnfinishedSessionForVokiData;
		takeVokiPageLink: string;
		replaceStateOnGoto: boolean;
	}
	let { sessionData, takeVokiPageLink, replaceStateOnGoto }: Props = $props();

	function onContinueBtnClick() {
		VokiTakingSessionMarkerCookie.markContinueFor2Min(sessionData.vokiId, sessionData.sessionId);
		goto(`${takeVokiPageLink}?continueExistingUnfinishedSession=true`, {
			replaceState: replaceStateOnGoto
		});
	}
	function onTerminateBtnClick() {
		VokiTakingSessionMarkerCookie.markTerminateFor2Min(sessionData.vokiId, sessionData.sessionId);
		goto(`${takeVokiPageLink}?terminateExistingUnfinishedSession=true`, {
			replaceState: replaceStateOnGoto
		});
	}
</script>

<div class="unfinished-session-container">
	<h1>Resume Session</h1>

	<div class="session-details">
		<div class="detail-item">
			<span class="label">Started</span>
			<span class="value" use:relativeTime={{ date: sessionData.startedAt }}></span>
		</div>
		<div class="separator"></div>
		<div class="detail-item">
			<span class="label">Progress</span>
			<span class="value"
				>{sessionData.questionsWithSavedAnswersCount} / {sessionData.totalQuestionsCount}</span
			>
		</div>
	</div>

	<p class="description">
		You have an unfinished session. Would you like to continue where you left off or start fresh?
	</p>

	<div class="actions">
		<PrimaryButton onclick={onContinueBtnClick} class="continue-btn">Continue Session</PrimaryButton
		>
		<button class="terminate-btn" onclick={onTerminateBtnClick}> Start New Session </button>
	</div>
</div>

<style>
	.unfinished-session-container {
		display: flex;
		flex-direction: column;
		align-items: center;
		max-width: 32rem;
		gap: 0.5rem;
	}

	h1 {
		font-size: 1.75rem;
		font-weight: 600;
		color: var(--text);
		margin: 0;
	}

	.session-details {
		display: grid;
		grid-template-columns: 1fr auto 1fr;
		align-items: center;
		justify-content: center;
		gap: 1.5rem;
		width: 100%;
		padding: 1rem;
		background-color: var(--secondary);
		border-radius: 0.75rem;
	}

	.detail-item {
		display: flex;
		flex-direction: column;
		align-items: center;
		gap: 0.25rem;
	}

	.separator {
		width: 1px;
		height: 2rem;
		background-color: var(--muted-foreground);
		opacity: 0.2;
	}

	.label {
		font-size: 0.875rem;
		color: var(--muted-foreground);
		text-transform: uppercase;
		letter-spacing: 0.05em;
		font-weight: 500;
	}

	.value {
		font-size: 1.125rem;
		font-weight: 600;
		color: var(--text);
	}

	.description {
		font-size: 1rem;
		line-height: 1.25;
		color: var(--secondary-foreground);
		margin: 0.5rem 0;
		text-align: center;
	}

	.actions {
		display: flex;
		flex-direction: column;
		gap: 1rem;
		width: 100%;
		margin-top: 1.5rem;
	}

	:global(.continue-btn) {
		width: 100% !important;
		justify-content: center;
	}

	.terminate-btn {
		width: 100%;
		padding: 0.75rem;
		border: none;
		background: none;
		color: var(--muted-foreground);
		font-size: 1rem;
		font-weight: 500;
		cursor: pointer;
		transition: all 0.2s ease;
		border-radius: 0.5rem;
	}

	.terminate-btn:hover {
		color: var(--red-5);
		background-color: var(--red-1);
	}
</style>
