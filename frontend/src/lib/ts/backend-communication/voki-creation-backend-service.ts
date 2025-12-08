import type { Language } from "../language";
import { BackendService, RJO } from "./backend-services";
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
    hasMatureContent: boolean;
}
export type VokiPublishingIssueType = 'Problem' | 'Warning';
export type VokiPublishingIssue = {
    type: VokiPublishingIssueType;
    message: string;
    source: string;
    fixRecommendation: string;

}
type VokiPublishingIssuesList = { issues: VokiPublishingIssue[] }
export type VokiSuccessfullyPublishedData = {
    id: string;
    cover: string;
    name: string;
}
export interface IVokiCreationBackendService {
    setVokiCoverToDefault(vokiId: string): Promise<ResponseResult<{ newCover: string; }>>;
    updateVokiCover(vokiId: string, newCover: string): Promise<ResponseResult<{ newCover: string; }>>;
    updateVokiName(vokiId: string, newName: string): Promise<ResponseResult<{ newName: string; }>>;
    getVokiName(vokiId: string): Promise<ResponseResult<{ vokiId: string, vokiName: string; }>>;
    updateVokiTags(vokiId: string, tags: string[]): Promise<ResponseResult<{ newTags: string[]; }>>;
    updateVokiDetails(vokiId: string, details: VokiDetails): Promise<ResponseResult<VokiDetails>>;
    checkForPublishingIssues(vokiId: string): Promise<ResponseResult<VokiPublishingIssuesList>>;
    publish(vokiId: string): Promise<ResponseResult<
        VokiSuccessfullyPublishedData | VokiPublishingIssuesList
    >>;
    publishWithWarningsIgnored(vokiId: string): Promise<ResponseResult<VokiSuccessfullyPublishedData>>;
}
class VokiCreationBackendService extends BackendService implements IVokiCreationBackendService {
    constructor(baseUrl: string) {
        super(baseUrl);
    }



    public async setVokiCoverToDefault(vokiId: string): Promise<ResponseResult<{ newCover: string; }>> {
        return await this.fetchJsonResponse<{ newCover: string }>(
            `/vokis/${vokiId}/set-cover-to-default`,
            { method: 'PATCH' }
        );
    }

    public async updateVokiCover(vokiId: string, newCover: string): Promise<ResponseResult<{ newCover: string; }>> {
        return await this.fetchJsonResponse<{ newCover: string; }>(
            `/vokis/${vokiId}/update-cover`,
            RJO.PATCH({ newCover: newCover })
        );
    }

    public async updateVokiName(vokiId: string, newName: string): Promise<ResponseResult<{ newName: string; }>> {
        return await this.fetchJsonResponse<{ newName: string; }>(
            `/vokis/${vokiId}/update-name`,
            RJO.PATCH({ newName })
        );
    }
    public async getVokiName(vokiId: string): Promise<ResponseResult<{ vokiId: string, vokiName: string; }>> {
        return await this.fetchJsonResponse<{ vokiId: string, vokiName: string; }>(
            `/vokis/${vokiId}/voki-name`, { method: 'GET' }
        );
    }


    public async updateVokiTags(vokiId: string, newTags: string[]): Promise<ResponseResult<{ newTags: string[]; }>> {
        return await this.fetchJsonResponse<{ newTags: string[]; }>(
            `/vokis/${vokiId}/update-tags`,
            RJO.PATCH({ newTags })
        );
    }

    public async updateVokiDetails(vokiId: string, details: VokiDetails): Promise<ResponseResult<VokiDetails>> {
        return await this.fetchJsonResponse<VokiDetails>(
            `/vokis/${vokiId}/update-details`,
            RJO.PATCH({
                newDescription: details.description,
                newLanguage: details.language,
                newHasMatureContent: details.hasMatureContent
            })
        );
    }
    public async checkForPublishingIssues(vokiId: string): Promise<ResponseResult<VokiPublishingIssuesList>> {
        return await this.fetchJsonResponse<{ issues: VokiPublishingIssue[]; }>(
            `/vokis/${vokiId}/publishing-issues`, { method: 'GET' }
        );
    }
    public async publish(vokiId: string): Promise<ResponseResult<VokiSuccessfullyPublishedData | VokiPublishingIssuesList>> {
        return await this.fetchJsonResponse<VokiSuccessfullyPublishedData | VokiPublishingIssuesList>(
            `/vokis/${vokiId}/publish`, RJO.POST({})
        );
    }
    public async publishWithWarningsIgnored(vokiId: string): Promise<ResponseResult<VokiSuccessfullyPublishedData>> {
        return await this.fetchJsonResponse<VokiSuccessfullyPublishedData>(
            `/vokis/${vokiId}/publish-with-warnings-ignored`, RJO.POST({})
        );
    }
}

export const ApiVokiCreationGeneral = new VokiCreationBackendService('/api/voki-creation/general');
