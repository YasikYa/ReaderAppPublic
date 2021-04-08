import { AnyAction, Middleware } from '@reduxjs/toolkit';
import { setLoadingFlag, setIsLoading } from 'store/app';

export const apiObserverMiddleware: Middleware = ({ dispatch }) => (next) => (
    action: AnyAction
) => {
    const { type } = action;
    // if action type includes string "fetch", then async action was called
    // example "app/fetchAllInitialData/pending"
    if (type.includes('fetch')) {
        dispatch(setIsLoading(type.includes('pending')));
        // slice typePrefix of async action
        const typePrefix = type.split('/').slice(0, 2).join('/');
        // if type includes string "pending", then async action just loading
        // else finished execution
        dispatch(
            setLoadingFlag({
                actionType: typePrefix,
                value: type.includes('pending'),
            })
        );
    }

    return next(action);
};
