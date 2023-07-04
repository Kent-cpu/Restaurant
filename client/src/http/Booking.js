import {$authHost} from "./index";

export const createBooking = async (UserID, TableID, Date, TimeFrom, TimeTo) => {
    const {data} = await $authHost.post("api/Bookings", {UserID, TableID, Date, TimeFrom, TimeTo});
    return data;
}

export const getBooking = async (userId) => {
    const {data} = await $authHost.get(`api/Bookings/mybookings/${userId}`);
    return data;
}

export const getAllBooking = async () => {
    const {data} = await $authHost.get("api/Bookings");
    return data;
}

export const getUserHistoryBooking = async (userId) => {
    const {data} = await $authHost.get(`api/Bookings/mybookings/history/${userId}`);
    return data;
}

export const deleteBooking = async (id) => {
    const response = await $authHost.delete(`api/Bookings/${id}`);
    return response;
}