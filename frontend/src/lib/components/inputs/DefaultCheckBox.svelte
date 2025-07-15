<script lang="ts">
	import { StringUtils } from '$lib/ts/utils/string-utils';

	let {
		checked = $bindable(),
		id = StringUtils.rndStrWithPref('checkbox-')
	}: { checked: boolean; id: string } = $props<{
		checked: boolean;
		id?: string;
	}>();
</script>

<div class="checkbox-wrapper-46">
	<input type="checkbox" {id} class="input" />
	<label for={id} class="cbx"
		><span>
			<svg viewBox="0 0 12 10" height="10px" width="12px">
				<polyline points="1.5 6 4.5 9 10.5 1"></polyline></svg
			></span
		></label
	>
</div>

<style>
	input {
		display: none;
		visibility: hidden;
	}

	.cbx {
		margin: auto;
		-webkit-user-select: none;
		user-select: none;
		cursor: pointer;
	}
	.cbx span {
		display: inline-block;
		vertical-align: middle;
		transform: translate3d(0, 0, 0);
	}
	.cbx span {
		position: relative;
		width: 18px;
		height: 18px;
		border-radius: 3px;
		transform: scale(1);
		vertical-align: middle;
		border: 1px solid #9098a9;
		transition: all 0.2s ease;
	}
	.cbx span svg {
		position: absolute;
		top: 3px;
		left: 2px;
		fill: none;
		stroke: #ffffff;
		stroke-width: 2;
		stroke-linecap: round;
		stroke-linejoin: round;
		stroke-dasharray: 16px;
		stroke-dashoffset: 16px;
		transition: all 0.3s ease;
		transition-delay: 0.1s;
		transform: translate3d(0, 0, 0);
	}
	.cbx span:before {
		content: '';
		width: 100%;
		height: 100%;
		background: #506eec;
		display: block;
		transform: scale(0);
		opacity: 1;
		border-radius: 50%;
	}

	.cbx:hover span {
		border-color: #506eec;
	}

	.input:checked + .cbx span {
		background: #506eec;
		border-color: #506eec;
		animation: wave-46 0.4s ease;
	}
	.input:checked + .cbx span svg {
		stroke-dashoffset: 0;
	}
	.input:checked + .cbx span:before {
		transform: scale(3.5);
		opacity: 0;
		transition: all 0.6s ease;
	}

	@keyframes wave-46 {
		50% {
			transform: scale(0.9);
		}
	}
</style>
