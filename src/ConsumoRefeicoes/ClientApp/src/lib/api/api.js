/* eslint-disable no-param-reassign */
import axios from 'axios';
import TokenAPI from './token';

const api = axios.create({  
  headers: {
    Accept: 'application/json',
  },
});

api.interceptors.request.use(async (config) => {
  const storage = TokenAPI.getToken();
  if (storage?.token) {
    config.headers.Authorization = `Bearer ${storage?.token}`;
  }
  return config;
});

api.interceptors.response.use(
  (response) => {
    return Promise.resolve(response);
  },
  (error) => {
    console.error(
      `${error.response?.status} - 
      ${error.response?.statusText} --> ${error.response?.data.error}`,
    );

    return error.response;
  },
);

export default api;
