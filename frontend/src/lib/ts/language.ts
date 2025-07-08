export type Language =
    | 'Eng'
    | 'Rus'
    | 'Spa'
    | 'Ger'
    | 'Fra'
    | 'Other';

export class LanguageUtils {
    public static name(language: Language): string {
        switch (language) {
            case 'Eng':
                return 'English';
            case 'Rus':
                return 'Russian';
            case 'Spa':
                return 'Spanish';
            case 'Ger':
                return 'German';
            case 'Fra':
                return 'French';
            case 'Other':
                return 'Other';
        }
    }
}