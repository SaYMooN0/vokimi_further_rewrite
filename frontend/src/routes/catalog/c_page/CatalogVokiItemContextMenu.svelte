<script lang="ts">
	import ContextMenuWithActions, {
		type ActionsContextMenuAction,
		type ActionsContextMenuActionsContent,
		type ActionsContextMenuMessageContent
	} from '$lib/components/context_menus/ContextMenuWithActions.svelte';
	import { AuthStore } from '$lib/ts/stores/auth-store.svelte';
	import type { PublishedVokiBriefInfo } from '$lib/ts/voki';
	import { watch } from 'runed';
	import { toast } from 'svelte-sonner';

	interface Props {
		removeVokiFromListInParent: (voki: PublishedVokiBriefInfo) => void;
	}

	let { removeVokiFromListInParent }: Props = $props();
	let contextMenu = $state<ContextMenuWithActions>()!;

	export function open(event: MouseEvent, voki: PublishedVokiBriefInfo) {
		currentVoki = voki;
		contextMenu.open(event.x, event.y, 8, -5);
	}
	let notAuthenticatedUserActions: () => ActionsContextMenuAction[] = () => [
		{
			label: 'Open',
			iconHref: '#context-menu-open-icon',
			action: {
				isLink: true,
				href: `/catalog/${currentVoki!.id}`
			},
			type: 'default'
		}
	];
	let authenticatedUserActions: () => ActionsContextMenuAction[] = () => [
		...notAuthenticatedUserActions(),
		{
			label: 'Add voki to album',
			iconHref: '#context-menu-add-to-album-icon',
			action: {
				isLink: false,
				onclick: () => {}
			},
			type: 'default'
		},
		{
			label: 'Not interested',
			iconHref: '#context-menu-not-interested-icon',
			action: {
				isLink: false,
				onclick: () => notInterestedAction(currentVoki!)
			},
			type: 'red'
		}
	];
	let currentVoki: PublishedVokiBriefInfo | undefined = $state<PublishedVokiBriefInfo>();
	let menuContent: ActionsContextMenuMessageContent | ActionsContextMenuActionsContent = $state({
		type: 'message',
		message: 'No album selected',
		iconHref: '#common-crossed-circle-icon'
	});
	watch(
		() => currentVoki,
		() => {
			if (!currentVoki) {
				menuContent = {
					type: 'message',
					message: 'No album selected',
					iconHref: '#common-crossed-circle-icon'
				};
			} else if (AuthStore.Get().isAuthenticated) {
				menuContent = {
					type: 'actions',
					items: authenticatedUserActions()
				};
			} else {
				menuContent = {
					type: 'actions',
					items: notAuthenticatedUserActions()
				};
			}
		}
	);

	function notInterestedAction(voki: PublishedVokiBriefInfo) {
		if (AuthStore.Get().isAuthenticated) {
			removeVokiFromListInParent(voki);
			contextMenu.close();
			toast.error('Not implemented');
		} else {
			toast.warning('You need to log in first');
		}
	}
</script>

<ContextMenuWithActions bind:this={contextMenu} content={menuContent} />
