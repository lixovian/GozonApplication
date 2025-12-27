<script setup lang="ts">
import { ref } from 'vue';
import { listOrders } from '../api/orders';
import StatusPill from '../components/StatusPill.vue';
import type { OrderListItem } from '../types/orders';

const props = defineProps<{ userId: number; toast: any }>();
const loading = ref(false);
const items = ref<OrderListItem[]>([]);

function cardTone(status: string) {
  const s = status.toLowerCase();
  if (s.includes('paid') || s.includes('success')) return 'ok';
  if (s.includes('fail') || s.includes('cancel') || s.includes('error')) return 'bad';
  return 'neutral';
}

async function load() {
  loading.value = true;
  try {
    items.value = await listOrders(props.userId);
    props.toast?.push('Список заказов обновлён', 'neutral', `Найдено: ${items.value.length}`);
  } catch (e: any) {
    props.toast?.push('Не удалось получить список заказов', 'bad', e?.body ?? e?.message);
  } finally {
    loading.value = false;
  }
}
</script>

<template>
  <div class="card">
    <div class="row" style="justify-content: space-between;">
      <div>
        <h2 class="h1" style="margin-bottom: 6px;">Список заказов</h2>
        <p class="sub">Горизонтальные карточки, цвет зависит от статуса.</p>
      </div>
      <div class="row">
        <button class="btn" :disabled="loading" @click="load">
          {{ loading ? 'Загрузка…' : 'Обновить' }}
        </button>
        <span class="badge">userId: <span class="mono">{{ userId }}</span></span>
      </div>
    </div>

    <hr />

    <div v-if="items.length === 0" class="sub">
      Пока пусто. Нажми “Обновить” или создай заказ.
    </div>

    <div class="hlist" v-else>
      <div
          v-for="o in items"
          :key="o.id"
          class="orderCard"
          :class="cardTone(o.status)"
      >
        <div class="row" style="justify-content: space-between;">
          <div class="badge mono">{{ o.id }}</div>
          <StatusPill :status="o.status" />
        </div>

        <div style="margin-top: 10px; font-size: 16px;">
          {{ o.description }}
        </div>

        <div class="kv">
          <div class="k">Сумма</div>
          <div class="v"><span class="mono">{{ o.amount }}</span></div>

          <div class="k">Создан</div>
          <div class="v">{{ o.createdAt ?? '—' }}</div>
        </div>
      </div>
    </div>
  </div>
</template>
