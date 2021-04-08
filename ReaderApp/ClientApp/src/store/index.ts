import { configureStore } from '@reduxjs/toolkit';
import { apiObserverMiddleware } from 'middlewares/apiObserverMiddleware';
import logger from 'redux-logger';
import { createSelectorHook, useDispatch as useAppDispatch } from 'react-redux';
import { appReducer } from './app';
import { authReducer } from './auth';
import { filesReducer } from './files';
import { wordsReduser } from './words';

export const store = configureStore({
    reducer: {
        app: appReducer,
        words: wordsReduser,
        auth: authReducer,
        files: filesReducer,
    },
    middleware: (getDefaultMiddleware) =>
        getDefaultMiddleware({
            serializableCheck: false,
        }).concat([apiObserverMiddleware, logger]),
});

export type RootState = ReturnType<typeof store.getState>;
export type AppDispatch = typeof store.dispatch;
export const useDispatch = () => useAppDispatch<AppDispatch>();
export const useSelector = createSelectorHook<RootState>();
