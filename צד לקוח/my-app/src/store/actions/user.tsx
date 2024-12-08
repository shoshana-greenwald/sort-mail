// ================== user

import { LOGIN, LOGOUT } from "../actionTypes";

// LOGIN
export const login = ({email, password}:any) => {
    return {
        type: LOGIN,
        payload: {email , password}
    }
};

// LOGOUT
export const logout = () => {

    return {
        type: LOGOUT
    }
};
