import axios from "axios";

const urlServer = "https://localhost:7060";

const $host = axios.create({
    baseURL: urlServer,
});

const $authHost = axios.create({
    baseURL: urlServer,
});

const errorHandler = (error) => {
    if (error.response && error.response.status === 401) {
        alert("Ваша сессия закончена, пожалуйста перезайдите в аккаунт");
        console.log('Токен просрочен');
        localStorage.removeItem("token");
    }
    return Promise.reject(error);
};

const authInterceptor = config => {
    config.headers.authorization = `Bearer ${localStorage.getItem('token')}`;
    return config;
}

$authHost.interceptors.request.use(authInterceptor);
$authHost.interceptors.response.use((response) => response, errorHandler);

export {
    $host,
    $authHost,
};