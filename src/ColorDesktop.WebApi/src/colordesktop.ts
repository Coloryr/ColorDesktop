import { ManagerState, WindowState, WindowTransparencyType } from "./enums"
import { IInstance, IInstanceHandel, IInstanceWindow } from "./instance"

var colordesktop_instance: IInstance | null = null
var colordesktop_handel: IInstanceHandel | null = null
var colordesktop_window: any = null
var colordesktop_windowhandel: IInstanceWindow | null = null

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

export function colordesktop_register(plugin: IInstance) {
    colordesktop_instance = plugin
}

export function colordesktop_update(data: string) {
    if (colordesktop_instance == null) {
        return
    }
    colordesktop_instance.update(JSON.parse(data))
}

export function colordesktop_ishandel(): boolean {
    if (colordesktop_instance == null) {
        return false
    }
    colordesktop_handel = colordesktop_instance.getHandel()
    return colordesktop_handel == null
}

export function colordesktop_move(x: bigint, y: bigint): ManagerState {
    if (colordesktop_handel == null) {
        return ManagerState.Fail
    }

    return colordesktop_handel.move(x, y)
}
export function colordesktop_resize(x: bigint, y: bigint): ManagerState {
    if (colordesktop_handel == null) {
        return ManagerState.Fail
    }

    return colordesktop_handel.resize(x, y)
}

export function colordesktop_setstate(state: WindowState): ManagerState {
    if (colordesktop_handel == null) {
        return ManagerState.Fail
    }

    return colordesktop_handel.setState(state)
}

export function colordesktop_settran(tran: WindowTransparencyType): ManagerState {
    if (colordesktop_handel == null) {
        return ManagerState.Fail
    }

    return colordesktop_handel.setTran(tran)
}

export function colordesktop_render() {
    if (colordesktop_instance == null) {
        return
    }
    colordesktop_instance.renderTick()
}

export function colordesktop_start() {
    if (colordesktop_instance == null) {
        return
    }
    colordesktop_windowhandel = new WindowHandel()
    colordesktop_instance.start(colordesktop_windowhandel)
}

export function colordesktop_stop() {
    if (colordesktop_instance == null) {
        return
    }
    colordesktop_instance.stop(colordesktop_windowhandel)
}