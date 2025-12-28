<script setup lang="ts">
import { reactive } from 'vue';

export type ToastTone = 'ok' | 'bad' | 'neutral';
export type Toast = { id: string; title: string; body?: string; tone: ToastTone };

const state = reactive<{ toasts: Toast[] }>({ toasts: [] });

function push(title: string, tone: ToastTone, body?: string) {
  const id = crypto.randomUUID();
  state.toasts.unshift({ id, title, body, tone });
  setTimeout(() => {
    state.toasts = state.toasts.filter(t => t.id !== id);
  }, 3500);
}

defineExpose({ push });
</script>

<template>
  <div class="toastWrap">
    <div v-for="t in state.toasts" :key="t.id" class="toast" :class="t.tone">
      <p class="toastTitle">{{ t.title }}</p>
      <p v-if="t.body" class="toastBody">{{ t.body }}</p>
    </div>
  </div>
</template>

<style scoped>
</style>
