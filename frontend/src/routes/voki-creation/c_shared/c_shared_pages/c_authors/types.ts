export type VokiCreationAuthorsInfo = {
    primaryAuthorId: string;
    coAuthorIds: string[];
    invitedForCoAuthorUserIds: string[];
    vokiCreationDate: Date;
    maxVokiCoAuthors: number;
}