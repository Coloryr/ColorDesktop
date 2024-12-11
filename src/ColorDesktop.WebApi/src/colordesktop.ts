import { ManagerState, WindowState, WindowTransparencyType } from "./enums"
import { IInstance, IInstanceHandel, IInstanceWindow, InstanceDataObj } from "./instance"

export var colordesktop_instance: IInstance | null = null
export var colordesktop_handel: IInstanceHandel | null = null
export var colordesktop_window: any = null
export var colordesktop_windowhandel: IInstanceWindow | null = null

export class WindowHandel implements IInstanceWindow {
    async setState(state: WindowState): Promise<void> {
        if (colordesktop_window != null) {
            await colordesktop_window.setState(state)
        }
    }
    async setTran(tran: WindowTransparencyType): Promise<void> {
        if (colordesktop_window != null) {
            await colordesktop_window.setTran(tran)
        }
    }
    async move(x: bigint, y: bigint): Promise<void> {
        if (colordesktop_window != null) {
            await colordesktop_window.move(x, y)
        }
    }
    async resize(x: bigint, y: bigint): Promise<void> {
        if (colordesktop_window != null) {
            await colordesktop_window.resize(x, y)
        }
    }
    async activate(): Promise<void> {
        if (colordesktop_window != null) {
            await colordesktop_window.activate()
        }
    }
    async show(): Promise<void> {
        if (colordesktop_window != null) {
            await colordesktop_window.show()
        }
    }
    async close(): Promise<void> {
        if (colordesktop_window != null) {
            await colordesktop_window.close()
        }
    }
}

export class ColorDesktop {
    /**
     * 获取实例
     * @returns 实例
     */
    getInstance(): IInstance | null {
        return colordesktop_instance
    }
    /**
     * 获取操作句柄
     * @returns 句柄
     */
    getHandel(): IInstanceHandel | null {
        return colordesktop_handel
    }
    /**
     * 获取窗口控制器
     * @returns 控制器
     */
    getWindow() {
        return colordesktop_window
    }
    /**
     * 设置窗口控制器
     * @param window 控制器
     */
    setWindow(window: any) {
        colordesktop_window = window
    }
    /**
     * 获取实例控制器
     * @returns 实例控制器
     */
    getWindowhandel(): IInstanceWindow | null {
        return colordesktop_windowhandel
    }
    /**
     * 注册ColorDestop显示实例
     * @param plugin 显示实例
     */
    register(plugin: IInstance) {
        colordesktop_instance = plugin
        console.log("[api info] colordesktop register")
    }
    /**
     * 更新配置（外部调用）
     * @param data 配置
     */
    update(data: InstanceDataObj) {
        if (colordesktop_instance == null) {
            console.log("[api error] update fail colordesktop_instance is null")
        }
        else {
            console.log("[api info] colordesktop update")
            colordesktop_instance.update(data)
        }
    }
    /**
     * 是否有实例句柄（外部调用）
     * @returns 结果
     */
    haveHandel(): boolean {
        if (colordesktop_instance == null) {
            console.log("[api error] ishandel fail colordesktop_instance is null")
            return false
        }
        console.log("[api info] call ishandel")
        colordesktop_handel = colordesktop_instance.getHandel()
        return colordesktop_handel == null
    }
    /**
     * 移动窗口（外部调用）
     * @param x X坐标
     * @param y Y坐标
     * @returns 移动结果
     */
    move(x: bigint, y: bigint): ManagerState {
        if (colordesktop_handel == null) {
            console.log("[api error] move fail colordesktop_handel is null")
            return ManagerState.Fail
        }
        console.log("[api] move to x:" + x + " y:" + y)
        return colordesktop_handel.move(x, y)
    }
    /**
     * 调整窗口大小（外部调用）
     * @param x 宽度
     * @param y 高度
     * @returns 移动结果
     */
    resize(x: bigint, y: bigint): ManagerState {
        if (colordesktop_handel == null) {
            console.log("[api error] resize fail colordesktop_handel is null")
            return ManagerState.Fail
        }
        console.log("[api] resize to x:" + x + " y:" + y)
        return colordesktop_handel.resize(x, y)
    }
    /**
     * 设置窗口状态（外部调用）
     * @param state 状态
     * @returns 设置结果
     */
    setState(state: WindowState): ManagerState {
        if (colordesktop_handel == null) {
            console.log("[api error] state set fail colordesktop_handel is null")
            return ManagerState.Fail
        }
        console.log("[api] set state to " + state)
        return colordesktop_handel.setState(state)
    }
    /**
     * 设置窗口透明（外部调用）
     * @param tran 透明效果
     * @returns 设置结果
     */
    setTran(tran: WindowTransparencyType): ManagerState {
        if (colordesktop_handel == null) {
            console.log("[api error] tran set fail colordesktop_handel is null")
            return ManagerState.Fail
        }
        console.log("[api] set tran to " + tran)
        return colordesktop_handel.setTran(tran)
    }
    /**
     * 渲染（外部调用）
     */
    render() {
        if (colordesktop_instance == null) {
            console.log("[api error] render fail colordesktop_render is null")
        }
        else {
            colordesktop_instance.renderTick()
        }
    }
    /**
     * 开始显示（外部调用）
     */
    start() {
        if (colordesktop_instance == null) {
            console.log("[api error] start fail colordesktop_instance is null")
        }
        else {
            console.log("[api] colordesktop call start")
            if (colordesktop_window == null) {
                console.log("[api error] colordesktop_window is null")
            }
            colordesktop_windowhandel = new WindowHandel()
            colordesktop_instance.start(colordesktop_windowhandel)
        }
    }
    /**
     * 结束显示（外部调用）
     */
    stop() {
        if (colordesktop_instance == null) {
            console.log("[api error] stop fail colordesktop_instance is null")
        }
        else {
            console.log("[api] colordesktop stop")
            colordesktop_instance.stop(colordesktop_windowhandel)
        }
    }
    /**
     * 打开设置页面（外部调用）
     */
    showSetting() {
        if (colordesktop_instance == null) {
            console.log("[api error] showSetting fail colordesktop_instance is null")
        }
        else {
            console.log("[api] colordesktop showSetting")
            colordesktop_instance.showSetting()
        }
    }
    /**
     * 关闭设置页面（外部调用）
     */
    closeSetting() {
        if (colordesktop_instance == null) {
            console.log("[api error] closeSetting fail colordesktop_instance is null")
        }
        else {
            console.log("[api] colordesktop closeSetting")
            colordesktop_instance.closeSetting()
        }
    }
    /**
     * 设置配置文件
     * @param name 文件名，不带.json
     * @param config 内容
     */
    async setConfig(name: string, config: any) {
        const response = await fetch('/setConfig', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                'file': name
            },
            body: JSON.stringify(config),
        });

        // 检查请求是否成功
        if (!response.ok) {
            throw new Error('Network response was not ok ' + response.statusText);
        }
    }
    /**
     * 获取配置文件
     * @param name 文件名，不带.json
     * @returns 内容
     */
    async getConfig(name: string): Promise<any> {
        const response = await fetch('/getConfig', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/x-www-form-urlencoded'
            },
            body: 'file=' + name
        });
        if (!response.ok) {
            throw new Error('Network response was not ok ' + response.statusText);
        }
        return await response.json();
    }
}

export var colordesktop: ColorDesktop

colordesktop = new ColorDesktop()

if (typeof window !== 'undefined') {
    window.colordesktop = colordesktop
}