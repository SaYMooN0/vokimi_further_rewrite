import type { AllowCoAuthorInvitesSettingValue } from "$lib/ts/users";

export type VokiCreationAuthorsInfo = {
    primaryAuthorId: string;
    coAuthorIds: string[];
    invitedForCoAuthorUserIds: string[];
    vokiCreationDate: Date;
    maxVokiCoAuthors: number;
}
export type UserPreviewWithInvitesSettings = {
    id: string;
    uniqueName: string;
    displayName: string;
    profilePic: string;
    AllowCoAuthorInvites: AllowCoAuthorInvitesSettingValue;
}