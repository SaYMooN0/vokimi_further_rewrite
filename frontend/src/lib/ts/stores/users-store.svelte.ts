import { ApiUserProfiles } from "../backend-communication/backend-services";

export type UserPreviewData = {
    id: string;
    name: string;
    profilePic: string;
}

export namespace UsersStore {

    export async function Get(id: string): Promise<UserPreviewData | null> {
        const response = await ApiUserProfiles.fetchJsonResponse<UserPreviewData>(
            `/users/${id}/preview`, { method: "GET" }
        );
        if (response.isSuccess && response.data) {
            return response.data;
        }

        return null;
    }

}