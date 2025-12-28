<script setup lang="ts">
import { ref, onMounted } from 'vue';
import { listOrders } from '../api/orders';
import StatusPill from '../components/StatusPill.vue';
import type { OrderListItem } from '../types/orders';

const props = defineProps<{ userId: number; toast: any }>();

const loading = ref(false);
const items = ref<OrderListItem[]>([]);

function cardTone(status: string) {
  const s = status.toLowerCase();
  if (s.includes('paid') || s.includes('success') || s.includes('finished')) return 'ok';
  if (s.includes('fail') || s.includes('cancel') || s.includes('error')) return 'bad';
  return 'neutral';
}

async function load(showToast = true) {
  loading.value = true;
  try {
    items.value = await listOrders(props.userId);
    if (showToast) {
      props.toast?.push(
          'Список заказов обновлен',
          'neutral',
          `Найдено: ${items.value.length}`
      );
    }
  } catch (e: any) {
    if (showToast) {
      props.toast?.push(
          'Не удалось получить список заказов',
          'bad',
          e?.body ?? e?.message
      );
    }
  } finally {
    loading.value = false;
  }
}

onMounted(() => {
  load(false);
});
</script>

<template>
  <div class="card">
    <div class="row" style="justify-content: space-between;">
      <div>
        <h2 class="h1" style="margin-bottom: 6px;">Список заказов</h2>
        <p class="sub">Горизонтальные карточки с динамическим цветом</p>
      </div>
      <div class="row">
        <button class="btn" :disabled="loading" @click="load()">
          {{ loading ? 'Загрузка…' : 'Обновить' }}
        </button>
        <span class="badge">
          userId: <span class="mono">{{ userId }}</span>
        </span>
      </div>
    </div>

    <hr />

    <div v-if="!loading && items.length === 0" class="sub">
      Пока пусто. Создай заказ или обнови список.
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
          <div class="v">
            <span class="mono">{{ o.amount }}</span>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>
