import {$authHost} from "./index";

export const getTables = async () => {
    const {data} = await $authHost.get("api/Tables");
    return data;
}

export const getFreeTable = async (date, timeFrom, timeTo) => {
    const {data} = await $authHost.get(`api/Tables/free?date=${date}&timeFrom=${timeFrom}&timeTo=${timeTo}`);
    return data;
}

export const getFreeTableAsc = async (date, timeFrom, timeTo) => {
    const {data} = await $authHost.get(`api/Tables/free/sorted/asc?date=${date}&timeFrom=${timeFrom}&timeTo=${timeTo}`);
    return data;
}

export const getFreeTableDesc = async (date, timeFrom, timeTo) => {
    const {data} = await $authHost.get(`api/Tables/free/sorted/desc?date=${date}&timeFrom=${timeFrom}&timeTo=${timeTo}`);
    return data;
}

export const createTable = async (Name, Capacity) => {
    const {data} = await $authHost.post("api/Tables", {Name, Capacity});
    return data;
}

export const deleteTable = async (id) => {
    const response = await $authHost.delete(`api/Tables/${id}`);
    return response;
}