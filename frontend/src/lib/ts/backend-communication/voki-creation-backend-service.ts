import type { Language } from "../language";
import { RequestJsonOptions } from "../request-json-options";
import { BackendService } from "./backend-services";
import type { ResponseResult } from "./result-types";

export type VokiMainInfo = {
    name: string;
    cover: string;
    tags: string[];
    details: VokiDetails;
}

export type VokiDetails = {
    description: string;
    language: Language;
    isAgeRestricted: boolean;
}

export interface IVokiCreationBackendService {
    setVokiCoverToDefault(vokiId: string): Promise<ResponseResult<{ newVokiCover: string; }>>;
    updateVokiCover(vokiId: string, file: File): Promise<ResponseResult<{ newCover: string; }>>;
    updateVokiName(vokiId: string, newName: string): Promise<ResponseResult<{ newName: string; }>>;
    updateVokiTags(vokiId: string, tags: string[]): Promise<ResponseResult<{ newTags: string[]; }>>;
    updateVokiDetails(vokiId: string, details: VokiDetails): Promise<ResponseResult<VokiDetails>>;
}
class VokiCreationBackendService extends BackendService implements IVokiCreationBackendService {
    constructor(baseUrl: string) {
        super(baseUrl);
    }

    public async setVokiCoverToDefault(vokiId: string): Promise<ResponseResult<{ newVokiCover: string; }>> {
        return await this.fetchJsonResponse<{ newVokiCover: string }>(
            `/vokis/${vokiId}/set-cover-to-default`,
            { method: 'PATCH' }
        );
    }

    public async updateVokiCover(vokiId: string, file: File): Promise<ResponseResult<{ newCover: string; }>> {
        const formData = new FormData();
        formData.append("file", file);

        return await this.fetchJsonResponse<{ newCover: string; }>(`/vokis/${vokiId}/cover`, {
            method: 'POST',
            body: formData
        });
    }

    public async updateVokiName(vokiId: string, newName: string): Promise<ResponseResult<{ newName: string; }>> {
        return await this.fetchJsonResponse<{ newName: string; }>(
            `/vokis/${vokiId}/update-name`,
            RequestJsonOptions.PATCH({newName })
        );
    }

    public async updateVokiTags(vokiId: string, newTags: string[]): Promise<ResponseResult<{ newTags: string[]; }>> {
        return await this.fetchJsonResponse<{ newTags: string[]; }>(
            `/vokis/${vokiId}/update-tags`,
            RequestJsonOptions.PATCH({ newTags })
        );
    }

    public async updateVokiDetails(vokiId: string, details: VokiDetails): Promise<ResponseResult<VokiDetails>> {
        return await this.fetchJsonResponse<VokiDetails>(
            `/vokis/${vokiId}/update-details`,
            RequestJsonOptions.PATCH({
                newDescription: details.description,
                newLanguage: details.language,
                newIsAgeRestricted: details.isAgeRestricted
            })
        );
    }
}

export const ApiVokiCreationGeneral = new VokiCreationBackendService('/api/voki-creation/general');
