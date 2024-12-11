import { createApp } from 'vue'
import './style.css'
import App from './App.vue'

import { colordesktop, IInstance, IInstanceWindow, InstanceDataObj, IInstanceHandel } from 'colordesktop-webapi'

class Desktop implements IInstance {
    start(_window: IInstanceWindow | null): void {
        console.log("colordesktop start")
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

createApp(App).mount('#app')
