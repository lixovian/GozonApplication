<script setup lang="ts">
import { ref } from 'vue';
import { getOrderStatus } from '../api/orders';
import StatusPill from '../components/StatusPill.vue';

const props = defineProps<{ userId: number; toast: any }>();

const orderId = ref('');
const status = ref<string | null>(null);
const loading = ref(false);

async function check() {  
  if (!orderId.value.trim()) {
    props.toast?.push('Укажи номер заказа', 'bad');
    return;
  }
  loading.value = true;
  try {
    const res = await getOrderStatus(props.userId, orderId.value.trim());
    status.value = res.status;
    props.toast?.push('Статус получен', 'ok', `status: ${res.status}`);
  } catch (e: any) {
    status.value = null;
    props.toast?.push('Не удалось получить статус', 'bad', e?.body ?? e?.message);
  } finally {
    loading.value = false;
  }
}
</script>

<template>
  <div class="card">
    <h2 class="h1">Статус заказа</h2>
    <p class="sub">Введи <span class="mono">orderId</span> и получи текущий статус.</p>

    <hr />

    <div class="grid grid-2">
      <div>
        <label class="label">Order ID</label>
        <input class="input mono" v-model="orderId" placeholder="например: 3f2c..."/>
      </div>

      <div class="card" style="padding: 14px; border-radius: 14px;">
        <div class="sub">Результат</div>
        <div style="margin-top: 10px;">
          <StatusPill v-if="status" :status="status" />
          <span v-else class="sub">—</span>
        </div>
      </div>
    </div>

    <div class="row" style="margin-top: 14px;">
      <button class="btn btn-primary" :disabled="loading" @click="check">
        {{ loading ? 'Проверяем…' : 'Проверить' }}
      </button>
      <span class="badge">userId: <span class="mono">{{ userId }}</span></span>
    </div>
  </div>
</template>
