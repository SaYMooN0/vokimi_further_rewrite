import { tick } from 'svelte';

export type AnswerRef = { id: string };

type Params = {
    answers: AnswerRef[];
    chooseAnswer: (id: string) => void;
    selector?: string;
    focusOnMount?: boolean;
    useSpacebarToChoose?: boolean;
};

export function answersKeyboardNav(node: HTMLElement, init: Params) {
    let params = {
        selector: '.answer',
        focusOnMount: true,
        useSpacebarToChoose: true,
        ...init
    };

    let items: HTMLElement[] = [];
    const refresh = () => {
        items = Array.from(node.querySelectorAll<HTMLElement>(params.selector!));
    };

    const mo = new MutationObserver(() => refresh());
    mo.observe(node, { childList: true, subtree: true });
    refresh();

    function focusCard(i: number) {
        if (!items.length) return;
        if (i < 0) i = items.length - 1;
        if (i >= items.length) i = 0;
        items[i]?.focus();
    }

    function onKeyDown(e: KeyboardEvent) {

        if (e.altKey) {
            e.preventDefault();
            return;
        }
        const active = document.activeElement as HTMLElement | null;
        const idx = active ? items.indexOf(active) : -1;

        switch (e.key) {
            case 'Enter': {
                if (idx >= 0) {
                    e.preventDefault();
                    params.chooseAnswer(params.answers[idx].id);
                }
                break;
            }
            case ' ': {
                if (params.useSpacebarToChoose && idx >= 0) {
                    e.preventDefault();
                    params.chooseAnswer(params.answers[idx].id);
                }
                break;
            }
            case 'ArrowRight':
            case 'ArrowDown':
                e.preventDefault();
                focusCard((idx >= 0 ? idx : -1) + 1);
                break;
            case 'ArrowLeft':
            case 'ArrowUp':
                e.preventDefault();
                focusCard((idx >= 0 ? idx : items.length) - 1);
                break;
        }
    }

    node.addEventListener('keydown', onKeyDown);

    if (params.focusOnMount) {
        tick().then(() => node.focus());
    }

    return {
        update(next: Params) {
            params = {
                selector: '.answer',
                focusOnMount: true,
                useSpacebarToChoose: true,
                ...next
            };
            refresh();
        },
        destroy() {
            mo.disconnect();
            node.removeEventListener('keydown', onKeyDown);
        }
    };
}
