<script lang="ts">
	import { relativeTime } from 'svelte-relative-time';
	import type { ExistingUnfinishedSessionForVokiData } from '$lib/ts/voki-taking-session';
	import { goto } from '$app/navigation';
	import { VokiTakingSessionMarkerCookie } from '$lib/ts/cookies/voki-taking-session-marker';

	interface Props {
		sessionData: ExistingUnfinishedSessionForVokiData;
		takeVokiPageLink: string;
	}
	let { sessionData, takeVokiPageLink }: Props = $props();
	function onContinueBtnClick(vokiId: string, sessionId: string) {
		VokiTakingSessionMarkerCookie.markContinueFor2Min(vokiId, sessionId);
		goto(`${takeVokiPageLink}?continueExistingUnfinishedSession=true`);
	}
	function onTerminateBtnClick() {
		VokiTakingSessionMarkerCookie.markTerminateFor2Min(sessionData.vokiId, sessionData.sessionId);
		goto(`${takeVokiPageLink}?terminateExistingUnfinishedSession=true`);
	}
</script>

<div class="existing-session-view-data">
	<div class="info">
		<label>
			Session started:
			<span use:relativeTime={{ date: sessionData.startedAt }} />
		</label>
		<label>
			Questions answered: {sessionData.questionsWithSavedAnswersCount} / {sessionData.totalQuestionsCount}
		</label>
	</div>
	<span class="note"
		>You can continue where you left off or terminate the current session and start a new one</span
	>
	<button
		class="continue-btn"
		onclick={() => onContinueBtnClick(sessionData.vokiId, sessionData.sessionId)}>Continue</button
	>
	<button class="terminate-btn" onclick={onTerminateBtnClick}>Terminate old and start new</button>
</div>

<style>
	.existing-session-view-data {
		display: flex;
		flex-direction: column;
		align-items: center;
		gap: 1.25rem;
		padding: 2rem 4rem;
		color: var(--text);
	}
</style>
