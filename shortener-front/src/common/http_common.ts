import axios from "axios";
import APP_ENV from "./env.ts";

const http_common = axios.create({
    baseURL: APP_ENV.BASE_URL,
    headers: {
        "Content-Type": "application/json",
    },
    withCredentials: true,
})
http_common.interceptors.request.use(
    response => response,
    async error => {
        const originalRequest = error.config;
        if (error.response.status === 401 && !originalRequest._retry) {
            originalRequest._retry = true;
            try {
                const url = `${APP_ENV.BASE_URL}/user/refresh`;
                console.log('Refreshing token at ' + url)
                const {data} = await axios.post(
                    url,
                    {},
                    {withCredentials: true}
                );
                axios.defaults.headers.common['Authorization']
                    = `Bearer ${data.accessToken}`;
                return axios(originalRequest);
            } catch (err) {
                // Handle logout if refresh token fails
                console.error('Refresh token expired or invalid');
                axios.defaults.headers.common.Authorization = '';
            }
        }
        return Promise.reject(error);
    }
)

export default http_common
