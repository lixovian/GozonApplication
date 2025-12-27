<script setup lang="ts">
defineProps<{ userId: number }>();
</script>

<template>
  <div class="card">
    <h2 class="h1">Главная</h2>
    <p class="sub">
      Используй вкладки сверху: создавай заказ, смотри список и статусы, управляй балансом.
      Все запросы идут через ApiGateway.
    </p>

    <hr />

    <div class="row">
      <span class="badge">Тема: Dark</span>
      <span class="badge">Gateway: <span class="mono">/orders</span> и <span class="mono">/payments</span></span>
      <span class="badge">Текущий userId: <span class="mono">{{ userId }}</span></span>
    </div>
  </div>
</template>
