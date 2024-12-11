<template>
  <component :is="currentComponent" />
</template>

<script lang="ts">
import { defineComponent, onMounted, onUnmounted, ref } from 'vue';
import ConfigPage from './pages/Setting.vue';
import DisplayPage from './pages/Display.vue';
import { setAppEventBus } from './eventBus';

export default defineComponent({
  name: 'MainComponent',
  components: {
    ConfigPage,
    DisplayPage,
  },
  setup() {
    const currentComponent = ref('DisplayPage');

    // 暴露方法供外部调用
    const switchToConfigPage = () => {
      currentComponent.value = 'ConfigPage';
    };

    const back = () => {
      currentComponent.value = 'DisplayPage';
    };

    onMounted(() => {
      setAppEventBus({
        switchToConfigPage: switchToConfigPage,
        back: back
      })
    });

    onUnmounted(() => {
      setAppEventBus(null)
    });

    return { currentComponent };
  }
});
</script>

<style scoped></style>