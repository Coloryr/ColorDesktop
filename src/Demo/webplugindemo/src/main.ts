import { createApp } from 'vue'
import './style.css'
import App from './App.vue'
import Setting from './pages/Setting.vue'

import { colordesktop, IInstance, IInstanceWindow, InstanceDataObj, IInstanceHandel } from 'colordesktop-webapi'
import { displayEvent, settingEvent } from './eventBus'

var create = false

class Desktop implements IInstance {
    showSetting(): void {
        if (create) {
            displayEvent?.switchToConfigPage()
        }
        else {
            createApp(Setting).mount('#app')
        }
    }
    async closeSetting(): Promise<void> {
        await settingEvent?.back()
        if (create) {
            displayEvent?.back()
        } else {

        }
    }
    start(_window: IInstanceWindow | null): void {
        console.log("colordesktop start")
        createApp(App).mount('#app')
        create = true
    }
    stop(_window: IInstanceWindow | null): void {
        console.log("colordesktop stop")
    }
    renderTick(): void {

    }
    update(_obj: InstanceDataObj): void {
        console.log("colordesktop update")
    }
    getHandel(): IInstanceHandel | null {
        return null
    }
}

colordesktop.register(new Desktop())

