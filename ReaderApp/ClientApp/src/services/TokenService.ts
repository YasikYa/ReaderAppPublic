import { TOKEN_NAME } from 'shared/constants';
import { Token } from 'api/auth/types';

class TokenService {
    private _token: Token | null | undefined;

    constructor() {
        const savedToken = localStorage.getItem(TOKEN_NAME);

        if (savedToken) {
            const parsedToken: Token = JSON.parse(savedToken);

            if (Math.floor(+new Date() / 1000) > parsedToken.expires_in) {
                this.removeToken();
            } else {
                this._token = parsedToken;
            }
        } else {
            this._token = null;
        }
    }

    public getToken(): string | null {
        if (this._token) {
            return this._token.access_token;
        }
        return null;
    }

    public setToken(tokenData: Token, rememberToken: boolean): void {
        tokenData.expires_in = Math.floor(+new Date() / 1000) + tokenData.expires_in;

        if (rememberToken) {
            localStorage.setItem(TOKEN_NAME, JSON.stringify(tokenData));
        }

        this._token = tokenData;
    }

    public removeToken(): void {
        localStorage.removeItem(TOKEN_NAME);
        this._token = null;
    }
}

export const tokenService = new TokenService();
