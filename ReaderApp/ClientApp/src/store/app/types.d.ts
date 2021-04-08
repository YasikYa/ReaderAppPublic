export type AppStore = {
    loadingFlags: { [actionType: string]: boolean };
    isGeneralLoading: boolean;
};