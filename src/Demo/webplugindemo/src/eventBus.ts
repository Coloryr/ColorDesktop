interface AppEventBus {
  switchToConfigPage: () => void;
  back: () => void;
}

interface SettingEventBus {
  back: () => Promise<void>;
}

export let displayEvent: AppEventBus | null = null;
export let settingEvent: SettingEventBus | null = null;

export function setAppEventBus(newEventBus: AppEventBus | null) {
    displayEvent = newEventBus;
}

export function setSettingEventBus(newEventBus: SettingEventBus | null) {
  settingEvent = newEventBus;
}