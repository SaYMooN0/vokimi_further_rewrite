import { ApiUserProfiles } from "../backend-communication/backend-services";
import type { Err } from "../err";

export type UserPreviewData = {
    id: string;
    name: string;
    profilePic: string;
}

export namespace UsersStore {
    export function Get(id: string) {
        let user = $state<UserPreviewDataWithState>({ state: 'loading' });
        ApiUserProfiles.fetchJsonResponse<UserPreviewData>(
            `/users/${id}/preview`,
            { method: 'GET' }
        ).then((response) => {
            if (response.isSuccess) {
                user.state = 'ok';
                (user as { state: 'ok'; data: UserPreviewData }).data = response.data;
            } else {
                user.state = 'errs';
                (user as { state: 'errs'; errs: Err[] }).errs = response.errs;
            }
            console.log(user);

        });

        return user;
    }


    type UserPreviewDataWithState =
        | { state: 'loading' }
        | { state: 'ok'; data: UserPreviewData }
        | { state: 'errs'; errs: Err[] };
}