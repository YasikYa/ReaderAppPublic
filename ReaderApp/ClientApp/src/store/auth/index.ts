import { createSlice } from '@reduxjs/toolkit';
import { tokenService } from 'services/TokenService';
import { fetchAccountInfo, fetchLogin } from './actions';
import { AuthStore } from './types';
import { toast } from 'react-toastify';
import { ErrorResponse } from 'api/types';

const initialState: AuthStore = {
    isAuthorized: Boolean(tokenService.getToken()),
    userInfo: null,
};

const authSlice = createSlice({
    name: 'auth',
    initialState,
    reducers: {
        logout: (state) => {
            state.isAuthorized = false;
            tokenService.removeToken();
        },
    },
    extraReducers: (builder) => {
        builder.addCase(fetchLogin.fulfilled, (state, { payload }) => {
            state.isAuthorized = true;
            tokenService.setToken(payload, true);
        });
        builder.addCase(fetchLogin.rejected, (_, { payload }) => {
            const error = payload as ErrorResponse;
            toast.error(error.Message);
        });
        builder.addCase(fetchAccountInfo.fulfilled, (state, { payload }) => {
            state.userInfo = payload.data;
        });
    },
});

export const { logout } = authSlice.actions;

export const authReducer = authSlice.reducer;
