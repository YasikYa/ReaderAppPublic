import { createAsyncThunk } from '@reduxjs/toolkit';
import { getAccountInfo, login, signUp } from 'api/auth';
import { LoginPayload, SignUpPayload } from 'api/auth/types';

export const fetchAccountInfo = createAsyncThunk('auth/fetchAccountInfo', () => getAccountInfo());

export const fetchLogin = createAsyncThunk(
    'auth/fetchLogin',
    async (payload: LoginPayload, { rejectWithValue }) => {
        try {
            return await login({ payload });
        } catch (e) {
            return rejectWithValue(e.data);
        }
    }
);

export const fetchSignUp = createAsyncThunk(
    'auth/fetchSignUp',
    async (payload: SignUpPayload, { dispatch }) => {
        const { email, password } = payload;

        await signUp({ payload });

        dispatch(fetchLogin({ email, password }));
    }
);
