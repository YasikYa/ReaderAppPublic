import { createSlice, PayloadAction } from '@reduxjs/toolkit';
import { AppStore } from './types';

const initialState: AppStore = {
    loadingFlags: {},
    isGeneralLoading: false,
};

const appSlice = createSlice({
    name: 'app',
    initialState,
    reducers: {
        setIsLoading(state, { payload }: PayloadAction<boolean | undefined>) {
            state.isGeneralLoading =
                typeof payload === 'boolean' ? payload : !state.isGeneralLoading;
        },
        setLoadingFlag(
            state,
            {
                payload,
            }: PayloadAction<{
                actionType: string;
                value: boolean;
            }>
        ) {
            const { actionType, value } = payload;
            state.loadingFlags[actionType] = value;
        },
    },
});

export const { setIsLoading, setLoadingFlag } = appSlice.actions;

export const appReducer = appSlice.reducer;
