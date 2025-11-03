type UserItemInListState =
    | { state: 'PrimaryAuthor' }
    | { state: 'CoAuthor' }
    | { state: 'AlreadyInvited' }
    | {
        state: 'CandidateToInvite';
        isUserInListToInvite: boolean;
        addToListToInvite: () => void;
        removeFromListToInvite: () => void;
    };