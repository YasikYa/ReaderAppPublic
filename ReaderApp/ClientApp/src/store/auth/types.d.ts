import { UserInfo } from "api/auth/types";

export type AuthStore = {
    isAuthorized: boolean,
    userInfo: UserInfo | null
}