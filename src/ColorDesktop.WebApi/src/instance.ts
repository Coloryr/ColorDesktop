import { ManagerState, PosEnum, WindowState, WindowTransparencyType } from "./enums"

export class MarginObj {
    public left: bigint
    public right: bigint
    public top: bigint
    public bottom: bigint

    constructor() {
        this.left = 0n
        this.right = 0n
        this.top = 0n
        this.bottom = 0n
    }

    static createWithMargin(margin: MarginObj): MarginObj {
        let obj = new MarginObj()
        obj.left = margin.left
        obj.right = margin.right
        obj.top = margin.top
        obj.bottom = margin.bottom

        return obj
    }

    static createWithNum(left: bigint, right: bigint, top: bigint, bottom: bigint): MarginObj {
        let obj = new MarginObj()
        obj.left = left
        obj.right = right
        obj.top = top
        obj.bottom = bottom

        return obj
    }

    static create(num: bigint): MarginObj {
        let obj = new MarginObj()
        obj.left = num
        obj.right = num
        obj.top = num
        obj.bottom = num

        return obj
    }
}

export class InstanceDataObj {
    public uuid: string = ""
    public nick: string = ""
    public plugin: string
    public pos: PosEnum = PosEnum.TopLeft
    public margin: MarginObj = MarginObj.create(5n)
    public tran: WindowTransparencyType = WindowTransparencyType.Transparent
    public diplay: bigint = 0n
    public topmoel: boolean = true

    constructor(plugin: string) {
        this.plugin = plugin
    }
}

/**
 * 实例窗口控制，用于实例控制窗口
 */
export interface IInstanceWindow {
    /**
     * 在最前显示
     */
    activate(): void
    /**
     * 显示
     */
    show(): void
    /**
     * 关闭
     */
    close(): void
    /**
     * 移动
     */
    move(x: bigint, y: bigint): void
    /**
     * 调整大小
     */
    resize(x: bigint, y: bigint): void
    /**
     * 调整状态
     */
    setState(state: WindowState): void
    /**
     * 调整透明
     */
    setTran(tran: WindowTransparencyType): void
}

/**
 * 实例句柄
 * 用于其他组件控制该实例
 */
export interface IInstanceHandel {
    /**
     * 移动窗口
     */
    move(x: bigint, y: bigint): ManagerState
    /**
     * 重新设置窗口大小
     */
    resize(x: bigint, y: bigint): ManagerState
    /**
     * 设置窗口状态
     */
    setState(state: WindowState): ManagerState
    /**
     * 设置窗口透明
     */
    setTran(level: WindowTransparencyType): ManagerState
}

/**
 * 显示实例
 */
export interface IInstance {
    /**
     * 实例启动
     * @param window 实例窗口
     */
    start(window: IInstanceWindow | null): void
    /**
     * 实例停止
     * @param window 实例窗口
     */
    stop(window: IInstanceWindow | null): void
    /**
     * 渲染
     */
    renderTick(): void
    /**
     * 配置更新
     */
    update(obj: InstanceDataObj): void
    /**
     * 获取窗口控制
     */
    getHandel(): IInstanceHandel | null
    /**
     * 显示设置
     * 打开实例设置界面
     */
    showSetting(): void
    /**
     * 关闭设置
     * 关闭实例设置界面
     */
    closeSetting() : void
}