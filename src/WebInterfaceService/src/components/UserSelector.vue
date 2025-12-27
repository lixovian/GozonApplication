<script setup lang="ts">
const modelValue = defineModel<number>();
</script>

<template>
  <label>
    User ID:
    <input type="number" v-model.number="modelValue" />
  </label>
  <hr />
</template>
