import { ManagerState, WindowState, WindowTransparencyType } from "./enums"
import { IInstance, IInstanceHandel, IInstanceWindow } from "./instance"

export var colordesktop_instance: IInstance | null = null
export var colordesktop_handel: IInstanceHandel | null = null
export var colordesktop_window: any = null
export var colordesktop_windowhandel: IInstanceWindow | null = null

export class WindowHandel implements IInstanceWindow {
    async activate(): Promise<void> {
        if (colordesktop_window != null) {
            await colordesktop_window.Activate()
        }
    }
    async show(): Promise<void> {
        if (colordesktop_window != null) {
            await colordesktop_window.Show()
        }
    }
    async close(): Promise<void> {
        if (colordesktop_window != null) {
            await colordesktop_window.Close()
        }
    }
}

export class ColorDesktop {
    getInstance(): IInstance | null {
        return colordesktop_instance
    }

    getHandel(): IInstanceHandel | null {
        return colordesktop_handel
    }

    getWindow() {
        return colordesktop_window
    }

    setWindow(window: any) {
        colordesktop_window = window
    }

    getWindowhandel(): IInstanceWindow | null {
        return colordesktop_windowhandel
    }

    register(plugin: IInstance) {
        colordesktop_instance = plugin
        console.log("[api info] colordesktop register")
    }

    update(data: string) {
        if (colordesktop_instance == null) {
            console.log("[api error] update fail colordesktop_instance is null")
            return
        }
        console.log("[api info] colordesktop update")
        colordesktop_instance.update(JSON.parse(data))
    }

    haveHandel(): boolean {
        if (colordesktop_instance == null) {
            console.log("[api error] ishandel fail colordesktop_instance is null")
            return false
        }
        console.log("[api info] call ishandel")
        colordesktop_handel = colordesktop_instance.getHandel()
        return colordesktop_handel == null
    }

    move(x: bigint, y: bigint): ManagerState {
        if (colordesktop_handel == null) {
            console.log("[api error] move fail colordesktop_handel is null")
            return ManagerState.Fail
        }
        console.log("[api] move to x:" + x + " y:" + y)
        return colordesktop_handel.move(x, y)
    }

    resize(x: bigint, y: bigint): ManagerState {
        if (colordesktop_handel == null) {
            console.log("[api error] resize fail colordesktop_handel is null")
            return ManagerState.Fail
        }
        console.log("[api] resize to x:" + x + " y:" + y)
        return colordesktop_handel.resize(x, y)
    }

    setState(state: WindowState): ManagerState {
        if (colordesktop_handel == null) {
            console.log("[api error] state set fail colordesktop_handel is null")
            return ManagerState.Fail
        }
        console.log("[api] set state to " + state)
        return colordesktop_handel.setState(state)
    }

    setTran(tran: WindowTransparencyType): ManagerState {
        if (colordesktop_handel == null) {
            console.log("[api error] tran set fail colordesktop_handel is null")
            return ManagerState.Fail
        }
        console.log("[api] set tran to " + tran)
        return colordesktop_handel.setTran(tran)
    }

    render() {
        if (colordesktop_instance == null) {
            console.log("[api error] render fail colordesktop_render is null")
            return
        }
        colordesktop_instance.renderTick()
    }

    start() {
        if (colordesktop_instance == null) {
            console.log("[api error] start fail colordesktop_instance is null")
            return
        }
        console.log("[api] colordesktop call start")
        if (colordesktop_window == null) {
            console.log("[api error] colordesktop_window is null")
        }
        colordesktop_windowhandel = new WindowHandel()
        colordesktop_instance.start(colordesktop_windowhandel)
    }

    stop() {
        if (colordesktop_instance == null) {
            console.log("[api error] stop fail colordesktop_instance is null")
            return
        }
        console.log("[api] colordesktop stop")
        colordesktop_instance.stop(colordesktop_windowhandel)
    }
}

export var colordesktop: ColorDesktop

colordesktop = new ColorDesktop()

if (typeof window !== 'undefined') {
    window.colordesktop = colordesktop
}