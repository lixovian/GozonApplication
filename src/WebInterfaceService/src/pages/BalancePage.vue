<script setup lang="ts">
import { ref } from 'vue';
import { addAccount, getBalance, topUpAccount } from '../api/payments';

const props = defineProps<{ userId: number; toast: any }>();

const balance = ref<number | null>(null);
const loading = ref(false);
const topUpAmount = ref(100);

async function refresh() {
  loading.value = true;
  try {
    const res = await getBalance(props.userId);
    balance.value = res.balance;
    props.toast?.push('Баланс обновлён', 'ok', `Баланс: ${res.balance}`);
  } catch (e: any) {
    // если у тебя 404/400 когда счёта нет — покажем подсказку
    balance.value = null;
    props.toast?.push('Счёт не найден', 'bad', 'Создай счёт кнопкой ниже');
  } finally {
    loading.value = false;
  }
}

async function createAccount() {
  loading.value = true;
  try {
    await addAccount(props.userId);
    props.toast?.push('Счёт создан', 'ok');
    await refresh();
  } catch (e: any) {
    props.toast?.push('Не удалось создать счёт', 'bad', e?.body ?? e?.message);
  } finally {
    loading.value = false;
  }
}

async function topUp() {
  loading.value = true;
  try {
    await topUpAccount(props.userId, topUpAmount.value);
    props.toast?.push('Баланс пополнен', 'ok', `+${topUpAmount.value}`);
    await refresh();
  } catch (e: any) {
    props.toast?.push('Не удалось пополнить баланс', 'bad', e?.body ?? e?.message);
  } finally {
    loading.value = false;
  }
}
</script>

<template>
  <div class="card">
    <div class="row" style="justify-content: space-between;">
      <div>
        <h2 class="h1" style="margin-bottom: 6px;">Баланс</h2>
        <p class="sub">Показ баланса + пополнение + кнопка обновления.</p>
      </div>
      <div class="row">
        <button class="btn" :disabled="loading" @click="refresh">
          {{ loading ? '…' : 'Обновить баланс' }}
        </button>
        <span class="badge">userId: <span class="mono">{{ userId }}</span></span>
      </div>
    </div>

    <hr />

    <div class="grid grid-2">
      <div class="card" style="padding: 14px; border-radius: 14px;">
        <div class="sub">Текущий баланс</div>
        <div style="margin-top: 8px; font-size: 28px; letter-spacing: 0.2px;">
          <span v-if="balance !== null">{{ balance }}</span>
          <span v-else style="color: rgba(255,255,255,0.55);">—</span>
        </div>
        <div v-if="balance === null" class="sub" style="margin-top: 8px;">
          Похоже, счёта нет. Создай счёт ниже.
        </div>
      </div>

      <div>
        <label class="label">Сумма пополнения</label>
        <input class="input" type="number" v-model.number="topUpAmount" min="1" />

        <div class="row" style="margin-top: 12px;">
          <button class="btn btn-primary" :disabled="loading" @click="topUp">Пополнить</button>
          <button class="btn" :disabled="loading" @click="createAccount">Создать счёт</button>
        </div>

        <p class="sub" style="margin-top: 10px;">
          Если пополнение падает — проверь, что счёт создан и суммы корректны.
        </p>
      </div>
    </div>
  </div>
</template>
