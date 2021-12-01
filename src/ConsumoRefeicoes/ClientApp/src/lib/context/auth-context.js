import React, { createContext, useState } from 'react';

// const initicalState = {
//             username: "",
//             name: "",
//             groups: [
//             ]
//         };

const AuthContext = createContext();

const AuthProvider = ({ children }) => {
  const [dadosUser, setDadosUser] = useState();

  return (
    <AuthContext.Provider value={{ dadosUser, setDadosUser }}>
      {children}
    </AuthContext.Provider>
  );
};

export { AuthContext, AuthProvider };
