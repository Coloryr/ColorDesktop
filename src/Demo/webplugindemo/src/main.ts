/* eslint-disable no-var */
/* eslint-disable @typescript-eslint/no-unused-vars */
import './assets/main.css'

import { createApp } from 'vue'
import App from './App.vue'

import { InstanceDataObj, type IInstance, type IInstanceHandel, type IInstanceWindow } from 'colordesktop-webapi'

import { colordesktop_register } from 'colordesktop-webapi'

export class DemoInstance implements IInstance {
  start(window: IInstanceWindow | null): void {
    console.log('instance start')
  }
  stop(window: IInstanceWindow | null): void {
    console.log('instance stop')
  }
  renderTick(): void {

  }
  update(obj: InstanceDataObj): void {
    console.log('instance update')
  }
  getHandel(): IInstanceHandel | null {
    return null
  }
}

var demo: DemoInstance = new DemoInstance()

colordesktop_register(demo)

createApp(App).mount('#app')
