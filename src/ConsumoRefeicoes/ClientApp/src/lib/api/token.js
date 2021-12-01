const storageKey = process.env.REACT_APP_STORAGE_KEY;

const TokenAPI = {
  getToken: () => {
    return JSON.parse(localStorage.getItem(storageKey));
  },

  setToken: (token) => {
    localStorage.setItem(storageKey, JSON.stringify(token));
  },

  removeToken: () => {
    localStorage.removeItem(storageKey);
  },
};

export default TokenAPI;
