import type { VokiType } from "$lib/ts/voki"

type InviteVokiPreview = {
    vokiId: string,
    vokiName: string,
    vokiCover: string,
    vokiType: VokiType,
    primaryAuthorId: string,
    coAuthorIds: string[],
    invitedForCoAuthorUserIds: string[],
    creationDate: Date
}


//  string VokiId,
//     string VokiName,
//     string VokiCover,
//     VokiType VokiType,
//     string PrimaryAuthorId,
//     string[] CoAuthorsIds,
//     string[] InvitedForCoAuthorUserIds,
//     DateTime CreationDate