import { get, post } from 'services/HttpService';
import { RequestParams } from 'api/types';
import { LoginPayload, SignUpPayload, Token, UserInfo } from './types';

export const login = async ({ payload: { email, password } }: RequestParams<LoginPayload>) => {
    const { data: token } = await post<Token>('/api/Users/token', {
        email,
        password,
    });

    return token;
};

export const signUp = ({ payload }: RequestParams<SignUpPayload>) =>
    post('/api/Users/signup', payload);

export const getAccountInfo = () => get<UserInfo>('/api/Users/me');
