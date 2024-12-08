import { IInstance, IInstanceHandel } from "./instance"

var colordesktop_instance: IInstance | null = null
var colordesktop_handel: IInstanceHandel | null = null

function colordesktop_register(plugin: IInstance) {
    colordesktop_instance = plugin
}

function colordesktop_update(data: string) {
    if (colordesktop_instance == null) {
        return;
    }
}

function colordesktop_ishandel(): boolean {
    if (colordesktop_instance == null) {
        return false;
    }
    colordesktop_handel = colordesktop_instance.getHandel()
    return colordesktop_handel == null
}