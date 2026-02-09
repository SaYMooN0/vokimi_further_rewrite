<script lang="ts">
	import { goto } from '$app/navigation';
	import { ContinueVokiTakingSessionMarkerCookie } from '$lib/ts/cookies/continue-voki-taking-session-marker';

	interface Props {
		vokiId: string;
		questionsWithSavedAnswersCount: number;
		sessionId: string;
		startedAt: string;
		totalQuestionsCount: number;
	}
	let { questionsWithSavedAnswersCount, sessionId, startedAt, totalQuestionsCount, vokiId }: Props =
		$props();

	let takeVokiPageLink = `/take-voki/${vokiId}/general`;
	function onContinueBtnClick() {
		ContinueVokiTakingSessionMarkerCookie.markFor2Min(vokiId, sessionId);
		goto(`${takeVokiPageLink}?continueExistingUnfinishedSession=true`);
	}
	function onTerminateBtnClick() {
		goto(`${takeVokiPageLink}?terminateSession=true`);
	}
</script>

<div>
	<h1>
		It seems like you wanted to start a new Voki taking session, but you already have an unfinished
		session
	</h1>
	<p>You can continue the existing session or start a new one</p>
	<button onclick={onContinueBtnClick}>Continue existing session</button>
	<button onclick={onTerminateBtnClick}>Start new session</button>
</div>
