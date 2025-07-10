import type { Language } from "$lib/ts/language";

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