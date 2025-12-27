<script setup lang="ts">
import { ref } from 'vue';
import { createOrder } from '../api/orders';

const props = defineProps<{ userId: number; toast: any }>();

const amount = ref<number>(100);
const description = ref<string>('New Year order');
const loading = ref(false);

async function submit() {
  loading.value = true;
  try {
    const res = await createOrder(props.userId, amount.value, description.value);
    props.toast?.push('Заказ создан', 'ok', `orderId: ${res.orderId}`);
  } catch (e: any) {
    props.toast?.push('Не удалось создать заказ', 'bad', e?.body ?? e?.message);
  } finally {
    loading.value = false;
  }
}
</script>

<template>
  <div class="card">
    <h2 class="h1">Создать заказ</h2>
    <p class="sub">Создание заказа асинхронно запускает оплату.</p>

    <hr />

    <div class="grid grid-2">
      <div>
        <label class="label">Сумма</label>
        <input class="input" type="number" v-model.number="amount" min="1" />
      </div>
      <div>
        <label class="label">Описание</label>
        <input class="input" v-model="description" />
      </div>
    </div>

    <div class="row" style="margin-top: 14px;">
      <button class="btn btn-primary" :disabled="loading" @click="submit">
        {{ loading ? 'Создаём…' : 'Создать заказ' }}
      </button>
      <span class="badge">userId: <span class="mono">{{ userId }}</span></span>
    </div>
  </div>
</template>
