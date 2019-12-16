import axios from "axios";
import LoginStore from "../stores/LoginStore"

// Add a request interceptor
axios.interceptors.request.use(
  function (config) {
    const token = localStorage.getItem("authorization")

    if (config.url.includes("/oauth/token"))
      return config;

    if (token != null) {
      config.headers.Authorization = `Bearer ${token}`;
    } else if (window.location.pathname != "/login") {
      window.location.replace("/login");
    }

    return config;
  },
  function (error) {
    return Promise.reject(error);
  }
);

// Add a response interceptor
axios.interceptors.response.use(
  function (response) {
    // Do something with response data
    return response;
  },
  function (error) {
    // Do something with response error
    if (error.response.hasOwnProperty(status) && error.response.status === 401 && window.location.pathname != "/login") {
      LoginStore.logout()
      window.location.replace("/login");
    }
    return Promise.reject(error);
  }
);