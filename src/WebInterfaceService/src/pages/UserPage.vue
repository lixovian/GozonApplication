<script setup lang="ts">
const props = defineProps<{ userId: number }>();
const emit = defineEmits<{ (e: 'update:userId', v: number): void }>();
</script>

<template>
  <div class="card">
    <h2 class="h1">Текущий пользователь</h2>
    <p class="sub">Укажи <span class="mono">userId</span> текущего пользователя</p>

    <hr />

    <div class="grid grid-2">
      <div>
        <label class="label">User ID</label>
        <input class="input" type="number" :value="props.userId" @input="emit('update:userId', Number(($event.target as HTMLInputElement).value))" />
      </div>
    </div>
  </div>
</template>
