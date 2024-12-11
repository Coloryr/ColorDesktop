<template>
    <div>
        <h1>配置页面</h1>
        <form @submit.prevent="submitConfig">
            <div>
                <label for="key">文本设置</label>
                <input id="key" v-model="configData.key" />
            </div>
            <button type="submit">保存</button>
        </form>
    </div>
</template>

<script lang="ts">
import { colordesktop } from 'colordesktop-webapi';
import { defineComponent, ref, onMounted, onUnmounted } from 'vue';
import { ConfigData } from '../config';
import { displayEvent, setSettingEventBus } from '../eventBus';

export default defineComponent({
    name: 'ConfigPage',
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

        // 提交配置数据
        const submitConfig = async () => {
            try {
                await colordesktop.setConfig('config', configData.value);
                displayEvent?.back()
            } catch (error) {
                console.error('提交配置数据失败:', error);
            }
        };

        onMounted(() => {
            fetchConfig()
            setSettingEventBus({
                back: submitConfig
            })
        });

        onUnmounted(() => {
            setSettingEventBus(null)
        })

        return {
            configData,
            submitConfig,
        };
    },
});
</script>

<style scoped>
/* 样式可根据需要进行调整 */
form {
    display: flex;
    flex-direction: column;
}

label {
    margin-top: 10px;
}

input {
    margin-bottom: 10px;
}

button {
    margin-top: 20px;
}
</style>