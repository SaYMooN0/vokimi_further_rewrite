import { ApiVokiRatings } from "$lib/ts/backend-communication/backend-services";
import type { Err } from "$lib/ts/err";
import { DateUtils } from "$lib/ts/utils/date-utils";
import type { RatingValueToCountType, VokiDailyRatingsSnapshot } from "../../types";


export class RatingsHistoryState {
	readonly vokiId: string;
	#publicationDate: Date;

	allSnapshotsState: AllSnapshotsState = $state({ type: 'loading' });

	dateFilter: {
		from: Date | null;
		to: Date | null;
	} = $state({
		from: null,
		to: null
	});

	constructor(vokiId: string) {
		this.vokiId = vokiId;
		this.#publicationDate = new Date();

		this.loadAllRatingsSnapshots();
	}

	async loadAllRatingsSnapshots() {
		this.allSnapshotsState = { type: 'loading' };

		const response = await ApiVokiRatings.fetchJsonResponse<{
			vokiPublicationDate: Date;
			snapshots: VokiDailyRatingsSnapshot[]
		}>(
			`/vokis/${this.vokiId}/manage/history`, { method: "GET" }
		)
		if (response.isSuccess) {
			this.allSnapshotsState = {
				type: 'ok',
				data: response.data.snapshots
			};
			this.#publicationDate = response.data.vokiPublicationDate;
		} else {
			this.allSnapshotsState = {
				type: 'errs',
				errs: response.errs
			};
		}
	}

	emptyDistribution(): RatingValueToCountType {
		return { 1: 0, 2: 0, 3: 0, 4: 0, 5: 0 };
	}

	snapshotsToShow: VokiDailyRatingsSnapshot[] = $derived.by(() => {
		if (this.allSnapshotsState.type !== 'ok') {
			return [];
		}

		const { from, to } = this.dateFilter;

		const sorted = [...this.allSnapshotsState.data]
			.map((s) => ({
				...s,
				Date: DateUtils.startOfDay(s.date)
			}))
			.sort((a, b) => a.Date.getTime() - b.Date.getTime());

		if (sorted.length === 0) {
			return [];
		}

		const startDate = DateUtils.startOfDay(from ?? this.#publicationDate);

		const endDate = DateUtils.startOfDay(to ?? sorted[sorted.length - 1].Date);

		const result: VokiDailyRatingsSnapshot[] = [];

		let currentDate = startDate;
		let lastKnownDistribution: RatingValueToCountType | null = null;

		while (currentDate <= endDate) {
			const found = sorted.find((s) => DateUtils.sameDay(s.Date, currentDate));

			if (found) {
				lastKnownDistribution = found.distribution;
				result.push(found);
			} else {
				if (
					lastKnownDistribution === null &&
					DateUtils.sameDay(currentDate, DateUtils.startOfDay(this.#publicationDate))
				) {
					result.push({
						date: currentDate,
						distribution: this.emptyDistribution()
					});
				} else if (lastKnownDistribution !== null) {
					result.push({
						date: currentDate,
						distribution: lastKnownDistribution
					});
				}
			}

			currentDate = DateUtils.addDays(currentDate, 1);
		}

		return result;
	});
}


type AllSnapshotsState =
	| { type: 'ok', data: VokiDailyRatingsSnapshot[] }
	| { type: 'loading' }
	| { type: 'errs', errs: Err[] }