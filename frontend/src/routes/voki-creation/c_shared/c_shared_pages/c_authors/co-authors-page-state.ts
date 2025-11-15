import { ApiVokiCreationCore, RJO, ApiUserProfiles } from "$lib/ts/backend-communication/backend-services";
import type { Err } from "$lib/ts/err";
import { watch } from "runed";
import type { UserPreviewWithInvitesSettings, UsersRecommendedToInvite, UserInviteState } from "./types";

export class CoAuthorsPageState {
    readonly vokiId: string;
    readonly maxCoAuthorsCount: number;

    primaryAuthorId: string;
    coAuthorIds: string[];
    invitedForCoAuthorUserIds: string[];
    coAuthorsDialogState: CoAuthorsInviteDialogState;

    constructor(vokiId: string, primaryAuthorId: string, coAuthorIds: string[], invitedForCoAuthorUserIds: string[], maxCoAuthorsCount: number) {
        this.vokiId = vokiId;
        this.maxCoAuthorsCount = maxCoAuthorsCount;

        this.primaryAuthorId = $state(primaryAuthorId);
        this.coAuthorIds = $state(coAuthorIds);
        this.invitedForCoAuthorUserIds = $state(invitedForCoAuthorUserIds);

        this.coAuthorsDialogState = new CoAuthorsInviteDialogState(vokiId);
    }
}

export class CoAuthorsInviteDialogState {
    readonly vokiId: string;


    searchBarInputVal = $state('');
    searchedUsers = $state<UserPreviewWithInvitesSettings[]>([]);

    usersChosenToInvite: UserPreviewWithInvitesSettings[] = $state([]);

    usersRecommendedToInvite: UsersRecommendedToInvite = $state({ state: 'loading' });

    savingErrs: Err[] = $state([]);
    isLoadingSave = $state(false)

    constructor(vokiId: string) {
        this.vokiId = vokiId;
        this.loadUsersRecommendedToInvite();
        watch(
            () => [this.searchBarInputVal, this.usersChosenToInvite],
            () => { this.savingErrs = []; }
        );
    }
    clearOnDialogOpen() {
        this.searchBarInputVal = '';
        this.searchedUsers = [];
        this.savingErrs = [];
        this.isLoadingSave = false;
    }

    addUserToInvite(user: UserPreviewWithInvitesSettings) {
        if (!this.usersChosenToInvite.some((u) => u.id === user.id)) {
            this.usersChosenToInvite = [...this.usersChosenToInvite, user];
        }
    }
    removeUserFromToInvite(user: UserPreviewWithInvitesSettings) {
        this.usersChosenToInvite = this.usersChosenToInvite.filter((u) => u.id !== user.id);
    }

    isUserInListToInvite(userId: string) {
        return this.usersChosenToInvite.some((u) => u.id === userId);
    }

    async confirmUsersInvite(onSuccessfullyConfirmed: (coAuthorIds: string[], invitedForCoAuthorUserIds: string[]) => void) {
        this.savingErrs = [];
        this.isLoadingSave = true;
        const response = await ApiVokiCreationCore.fetchJsonResponse<{
            coAuthorIds: string[];
            invitedForCoAuthorUserIds: string[];
        }>(
            `/vokis/${this.vokiId}/invite-co-authors`,
            RJO.POST({ userIds: this.usersChosenToInvite.map((u) => u.id) })
        );
        this.isLoadingSave = false;
        if (response.isSuccess) {
            onSuccessfullyConfirmed(response.data.coAuthorIds, response.data.invitedForCoAuthorUserIds);
            this.savingErrs = [];
            this.usersChosenToInvite = [];
        } else {
            this.savingErrs = response.errs;
        }
    }
    async loadUsersRecommendedToInvite() {
        this.usersRecommendedToInvite = { state: 'loading' };
        const response = await ApiUserProfiles.fetchJsonResponse<{
            users: UserPreviewWithInvitesSettings[];
        }>(`/users/recommended-for-co-author`, { method: 'GET' });

        if (response.isSuccess) {
            this.usersRecommendedToInvite = { state: 'ok', users: response.data.users };
        } else {
            this.usersRecommendedToInvite = { state: 'errs', errs: response.errs };
        }
    }

    getUserInviteState(user: UserPreviewWithInvitesSettings): UserInviteState {
        if (user.id === this.primaryAuthorId) {
            return { state: 'PrimaryAuthor' };
        } else if (this.coAuthorIds.includes(user.id)) {
            return { state: 'CoAuthor' };
        } else if (this.invitedForCoAuthorUserIds.includes(user.id)) {
            return { state: 'AlreadyInvited' };
        } else {
            return {
                state: 'CandidateToInvite',
                isUserInListToInvite: this.isUserInListToInvite(user.id),
                addToListToInvite: () => this.addUserToInvite(user),
                removeFromListToInvite: () => this.removeUserFromToInvite(user)
            };
        }
    }
}
