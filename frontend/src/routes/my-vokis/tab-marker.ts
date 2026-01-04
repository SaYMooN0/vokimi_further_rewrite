
export namespace MyVokisPageTabMarker {
    export const cookieName = "my-vokis-page-last-tab";
    const possibleValues = ["draft-vokis", "published-vokis", "vokis-to-manage"] as const;
    export type Tab = typeof possibleValues[number];
    export function isTab(tab: string): tab is Tab {
        return (possibleValues as readonly string[]).includes(tab);
    }
}
