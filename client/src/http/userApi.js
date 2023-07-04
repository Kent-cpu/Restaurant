import {$host} from "./index";
import jwt_decode from "jwt-decode";


export const registration = async (username, password, role) => {
    const {data} = await $host.post('api/user/register',
        {Username: username, Password: password, Role: role});
    localStorage.setItem('token', data.token);
    return jwt_decode(data.token);
}

export const login = async (username, password) => {
    const {data} = await $host.post('api/user/login', {Username: username, Password: password});
    localStorage.setItem('token', data.token);
    return jwt_decode(data.token);
}