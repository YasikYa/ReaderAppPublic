export type RequestParams<T = any> = {
    payload: T;
    signal?: AbortSignal;
};

export type ErrorResponse = {
    Message: stringl;
};
