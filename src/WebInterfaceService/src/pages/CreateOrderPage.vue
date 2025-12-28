<script setup lang="ts">
import { ref } from 'vue';
import { createOrder } from '../api/orders';

const props = defineProps<{ userId: number; toast: any }>();

const amount = ref<number>(0);
const description = ref<string>('');
const loading = ref(false);

async function submit() {
  loading.value = true;
  try {
    const res = await createOrder(props.userId, amount.value, description.value);
    props.toast?.push('Заказ создан', 'ok', `orderId: ${res.id}`);
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
    <p class="sub">Страница создания заказа</p>

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
        {{ loading ? 'Создаем…' : 'Создать заказ' }}
      </button>
      <span class="badge">userId: <span class="mono">{{ userId }}</span></span>
    </div>
  </div>
</template>
