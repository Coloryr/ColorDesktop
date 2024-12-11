<template>
  <div>
    <h1>展示页面</h1>
    <div v-if="configData">
      <p><strong>文本内容：</strong> {{ configData.key }}</p>
    </div>
  </div>
</template>

<script lang="ts">
import { colordesktop } from 'colordesktop-webapi';
import { defineComponent, ref, onMounted } from 'vue';
import { ConfigData } from '../config';

export default defineComponent({
  name: 'DisplayPage',
  setup() {
    const configData = ref<ConfigData>({ key: '' });

    // 获取配置数据
    const fetchConfig = async () => {
      try {
        const response = await colordesktop.getConfig('config');
        configData.value = response;
      } catch (error) {
        console.error('获取配置数据失败:', error);
      }
    };

    onMounted(fetchConfig);

    return {
      configData,
    };
  },
});
</script>

<style scoped>

</style>