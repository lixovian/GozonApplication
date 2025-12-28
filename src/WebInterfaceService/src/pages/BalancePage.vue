<script setup lang="ts">
import { ref, computed, onMounted } from 'vue';
import { addAccount, getBalance, topUpAccount } from '../api/payments';

const props = defineProps<{ userId: number; toast: any }>();

const balance = ref<number | null>(null);
const loading = ref(false);
const topUpAmount = ref(100);

const accountExists = ref<boolean | null>(null);

const hasAccount = computed(() => accountExists.value === true);
const checked = computed(() => accountExists.value !== null);

async function checkAccountOnOpen() {
  loading.value = true;
  try {
    const res = await getBalance(props.userId);
    balance.value = res.balance;
    accountExists.value = true;
  } catch (e: any) {
    balance.value = null;
    accountExists.value = false;
  } finally {
    loading.value = false;
  }
}

async function refresh(showToast = true) {
  loading.value = true;
  try {
    const res = await getBalance(props.userId);
    balance.value = res.balance;
    accountExists.value = true;
    if (showToast) props.toast?.push('Баланс обновлен', 'ok', `Баланс: ${res.balance}`);
  } catch (e: any) {
    balance.value = null;
    accountExists.value = false;
    if (showToast) props.toast?.push('Счет не найден', 'bad', 'Создай счет кнопкой ниже');
  } finally {
    loading.value = false;
  }
}

async function createAccount() {
  loading.value = true;
  try {
    await addAccount(props.userId);
    props.toast?.push('Счет создан', 'ok');

    accountExists.value = true;

    await refresh(false);
  } catch (e: any) {
    accountExists.value = false;
    props.toast?.push('Не удалось создать счет', 'bad', e?.body ?? e?.message);
  } finally {
    loading.value = false;
  }
}

async function topUp() {
  if (!hasAccount.value) {
    props.toast?.push('Сначала создай счет', 'bad', 'Без счета пополнение невозможно');
    return;
  }

  loading.value = true;
  try {
    await topUpAccount(props.userId, topUpAmount.value);
    props.toast?.push('Баланс пополнен', 'ok', `+${topUpAmount.value}`);
    await refresh(false);
  } catch (e: any) {
    props.toast?.push('Не удалось пополнить баланс', 'bad', e?.body ?? e?.message);
  } finally {
    loading.value = false;
  }
}

onMounted(() => {
  checkAccountOnOpen();
});
</script>

<template>
  <div class="card">
    <div class="row" style="justify-content: space-between;">
      <div>
        <h2 class="h1" style="margin-bottom: 6px;">Баланс</h2>
        <p class="sub">Показ баланса + пополнение + открытие счета</p>
      </div>
      <div class="row">
        <button class="btn" :disabled="loading" @click="refresh()">
          {{ loading ? '...' : 'Обновить баланс' }}
        </button>
        <span class="badge">userId: <span class="mono">{{ userId }}</span></span>
      </div>
    </div>

    <hr />

    <div class="grid grid-2">
      <div class="card" style="padding: 14px; border-radius: 14px;">
        <div class="sub">Текущий баланс</div>

        <div style="margin-top: 8px; font-size: 28px; letter-spacing: 0.2px;">
          <span v-if="hasAccount && balance !== null">{{ balance }}</span>
          <span v-else-if="!checked" style="color: rgba(255,255,255,0.55);">…</span>
          <span v-else style="color: rgba(255,255,255,0.55);">—</span>
        </div>

        <div v-if="checked && !hasAccount && !loading" class="sub" style="margin-top: 8px;">
          Похоже, счета нет. Создай счет ниже.
        </div>
      </div>

      <div>
        <label class="label">Сумма пополнения</label>
        <input class="input" type="number" v-model.number="topUpAmount" min="1" />

        <div class="row" style="margin-top: 12px;">
          <button class="btn btn-primary" :disabled="loading || !hasAccount" @click="topUp">
            Пополнить
          </button>

          <button v-if="checked && !hasAccount" class="btn" :disabled="loading" @click="createAccount">
            Создать счет
          </button>
        </div>

        <p class="sub" style="margin-top: 10px;">
          <span v-if="!checked">Проверяем наличие счета…</span>
          <span v-else-if="hasAccount">Счет найден - можно пополнять и обновлять баланс.</span>
          <span v-else>Сначала создай счет, затем станет доступно пополнение.</span>
        </p>
      </div>
    </div>
  </div>
</template>
