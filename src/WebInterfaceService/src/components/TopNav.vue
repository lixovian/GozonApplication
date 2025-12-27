<script setup lang="ts">
export type TabKey =
    | 'home'
    | 'create'
    | 'list'
    | 'balance'
    | 'status'
    | 'user';

const props = defineProps<{
  modelValue: TabKey;
  userId: number;
}>();

const emit = defineEmits<{
  (e: 'update:modelValue', v: TabKey): void;
}>();

const tabs: { key: TabKey; label: string; hint: string }[] = [
  { key: 'home', label: 'Главная', hint: 'быстрый обзор' },
  { key: 'create', label: 'Создать заказ', hint: 'новый заказ' },
  { key: 'list', label: 'Список заказов', hint: 'горизонтально' },
  { key: 'balance', label: 'Баланс', hint: 'пополнение' },
  { key: 'status', label: 'Статус заказа', hint: 'по номеру' },
  { key: 'user', label: 'Пользователь', hint: 'userId' },
];
</script>

<template>
  <div class="nav">
    <div class="brand">
      <div class="logo"></div>
      <div>
        <div class="title">gOzon Web</div>
        <div class="subtitle">v1.01r</div>
      </div>
    </div>

    <div class="tabs">
      <button
          v-for="t in tabs"
          :key="t.key"
          class="tab"
          :class="{ active: t.key === props.modelValue }"
          @click="emit('update:modelValue', t.key)"
      >
        <span class="tabLabel">{{ t.label }}</span>
        <span class="tabHint">{{ t.hint }}</span>
      </button>
    </div>
  </div>
</template>

<style scoped>
.nav {
  position: sticky;
  top: 0;
  z-index: 10;
  background: rgba(11, 15, 23, 0.72);
  backdrop-filter: blur(10px);
  border-bottom: 1px solid rgba(255,255,255,0.08);
  padding: 14px 18px;
  display: flex;
  gap: 16px;
  align-items: center;
  justify-content: space-between;
}

.brand {
  display: flex;
  align-items: center;
  gap: 12px;
  min-width: 240px;
}
.logo {
  width: 40px;
  height: 40px;
  border-radius: 14px;
  background:
      radial-gradient(14px 14px at 30% 30%, rgba(255,255,255,0.25), transparent 60%),
      linear-gradient(135deg, rgba(124,92,255,0.65), rgba(45,227,159,0.45));
  border: 1px solid rgba(255,255,255,0.10);
  box-shadow: 0 12px 30px rgba(0,0,0,0.25);
}
.title { font-size: 14px; letter-spacing: 0.2px; }
.subtitle { font-size: 12px; color: rgba(255,255,255,0.62); }

.tabs {
  display: flex;
  gap: 10px;
  flex-wrap: wrap;
  justify-content: flex-end;
}

.tab {
  border: 1px solid rgba(255,255,255,0.10);
  background: rgba(255,255,255,0.03);
  color: rgba(255,255,255,0.92);
  border-radius: 16px;
  padding: 10px 12px;
  cursor: pointer;
  display: grid;
  gap: 2px;
  min-width: 150px;
  text-align: left;
  transition: transform 0.08s ease, background 0.15s ease, border-color 0.15s ease;
}

.tab:hover {
  background: rgba(255,255,255,0.06);
  border-color: rgba(255,255,255,0.16);
}

.tab:active { transform: translateY(1px); }

.tab.active {
  border-color: rgba(124,92,255,0.55);
  background: linear-gradient(180deg, rgba(124,92,255,0.30), rgba(124,92,255,0.10));
}

.tabLabel { font-size: 13px; }
.tabHint { font-size: 11px; color: rgba(255,255,255,0.55); }
</style>
