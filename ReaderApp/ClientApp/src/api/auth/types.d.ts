export type Token = {
    access_token: string;
    expires_in: number;
};

export type LoginPayload = {
    email: string;
    password: string;
};

export type SignUpPayload = {
    name: string;
    email: string;
    password: string;
};

export type UserInfo = {
    id: string;
    email: string;
    name: string;
};
