<script lang="ts">
	let name = $state('');
	let mainColor = $state('');
	let secondColor = $state('');
	let icon = $state('');
   
</script>

<div class="new-album-creation">
	<input type="text" class="name-input" placeholder="Album name" bind:value={name} />
    <div class="color-and-icons">

    </div>
</div>

<style>

</style>