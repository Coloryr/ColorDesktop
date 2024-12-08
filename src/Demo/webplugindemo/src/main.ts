/* eslint-disable @typescript-eslint/no-unused-vars */

import './assets/main.css'

import { createApp } from 'vue'
import App from './App.vue'

import { type IWebPlugin } from "../../../ColorDesktop.WebApi/dist/iweb-plugin"
import { LanguageType } from '../../../ColorDesktop.WebApi/dist/enums';
import { InstanceDataObj, type IInstance } from '../../../ColorDesktop.WebApi/dist/instance';

export class DemoPlugin implements IWebPlugin {
    isCoreLib(): boolean {
        return false
    }
    havePluginSetting(): boolean {
        return true
    }
    haveInstanceSetting(): boolean {
        return true
    }
    loadLang(type: LanguageType): void {
        
    }
    makeInstances(obj: InstanceDataObj): IInstance {
        throw new Error('Method not implemented.');
    }
    createInstanceDefault(): InstanceDataObj {
        throw new Error('Method not implemented.');
    }
    getIcon(): string {
        throw new Error('Method not implemented.');
    }
    openSetting(instance?: unknown): void {
        throw new Error('Method not implemented.');
    }
    permissions(key: string, permission: string): boolean {
        throw new Error('Method not implemented.');
    }
    init(local: string, instancelocal: string): void {
        throw new Error('Method not implemented.');
    }
    enable(): void {
        throw new Error('Method not implemented.');
    }
    disable(): void {
        throw new Error('Method not implemented.');
    }
    stop(): void {
        throw new Error('Method not implemented.');
    }

}

createApp(App).mount('#app')
