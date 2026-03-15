export namespace IconUtils {
    export const AllAlbumIcons = [
        'albums-bookmark-1-icon',
        'albums-bookmark-2-icon',
        'albums-clock-1-icon',
        'albums-clock-2-icon',
        'albums-star-1-icon',
        'albums-star-2-icon'
    ];
    export const AllAuthorProfileLinkTypes = ['website', 'twitter', 'instagram', 'telegram', 'youtube', 'tiktok', 'other'];
    export type AuthorProfileLinkType = typeof AllAuthorProfileLinkTypes[number];
    const profileLinkIconIdByTypeMap: Record<AuthorProfileLinkType, string> = {
        'website': 'link-website-icon',
        'twitter': 'link-twitter-icon',
        'instagram': 'link-instagram-icon',
        'telegram': 'link-telegram-icon',
        'youtube': 'link-youtube-icon',
        'tiktok': 'link-tiktok-icon',
        'other': 'link-other-icon'
    };
    export function getProfileLinkIconIdByType(type: AuthorProfileLinkType): string {
        const iconId = profileLinkIconIdByTypeMap[type];
        if (!iconId) {
            throw new Error(`Unknown author profile link type: ${type}`);
        }
        return `#${iconId}`;
    }
}