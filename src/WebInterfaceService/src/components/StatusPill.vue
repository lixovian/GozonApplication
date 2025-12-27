<script setup lang="ts">
const props = defineProps<{ status: string }>();

function tone(status: string) {
  switch (status) {
    case 'FINISHED':
      return 'ok';
    case 'CANCELLED':
      return 'bad';
    case 'NEW':
    default:
      return 'neutral';
  }
}
</script>

<template>
  <span class="pill" :class="tone(props.status)">
    <span class="dot"></span>
    {{ props.status }}
  </span>
</template>

<style scoped>
.pill {
  display: inline-flex;
  align-items: center;
  gap: 8px;
  padding: 7px 10px;
  border-radius: 999px;
  font-size: 12px;
  border: 1px solid rgba(255,255,255,0.10);
  background: rgba(255,255,255,0.03);
  color: rgba(255,255,255,0.80);
}
.dot {
  width: 8px;
  height: 8px;
  border-radius: 999px;
  background: rgba(255,255,255,0.35);
}
.ok { border-color: rgba(45,227,159,0.32); }
.ok .dot { background: rgba(45,227,159,0.85); }

.bad { border-color: rgba(255,77,109,0.32); }
.bad .dot { background: rgba(255,77,109,0.85); }

.neutral { border-color: rgba(74,168,255,0.24); }
.neutral .dot { background: rgba(74,168,255,0.85); }
</style>
