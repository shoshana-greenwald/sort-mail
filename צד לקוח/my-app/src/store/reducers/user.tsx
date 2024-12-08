// מחזיק את הסטייט הגלובלי
// מגדיר סטייט התחלתי
// ומגדיר את צורת השינוי שלו - מבצע את הפעולות לשינוי

import { LOGIN, LOGOUT } from "../actionTypes";

// actions-רדיוסר מקבל את הסטייט הגלובל הקודם ואת האוביקט שהחזירה הפעולה ב
// מחזיר את הסטייט המעודכן
export const userReducer = (state:any = { email: null,password: null }, { type, payload }: any) => {
    switch (type) {
        case LOGIN:
            return {
                ...state,
                email: payload.email,
                password:payload.password
            };

        case LOGOUT:
            return {
                ...state,
                email:null,
                password :null
            };
        default:
            return state;

    }
}