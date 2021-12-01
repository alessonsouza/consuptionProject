import React, { useState, useContext } from 'react';
import { Redirect } from 'react-router-dom';

import { Alert, AlertTitle } from '@material-ui/lab';
import Header from './header';
import LoginForm from './login-form';
import { Box, BoxTitle } from '../../components/box-card';
import { LoaderContext } from '../../lib/context/loader-context';
import { AuthContext } from '../../lib/context/auth-context';
import AuthAPI from '../../lib/api/auth';

import './form-login.css';

const initialState = {
  username: '',
  password: '',
};

const Layout = () => {
  const [loginData, setLoginData] = useState(initialState);
  const [successLogin, setSuccessLogin] = useState(false);
  const [errorLogin, setErrorLogin] = useState({});

  const { setIsLoading } = useContext(LoaderContext);
  const { setDadosUser } = useContext(AuthContext);

  const submitForm = async (values) => {
    setIsLoading(true);

    setLoginData(values);
    const result = await AuthAPI.autenticate(values);
    setSuccessLogin(result.success);
    setDadosUser(result.data?.user);
    if (result.success === false) {
      setErrorLogin(result);
    }
    setIsLoading(false);
  };

  return (
    <>
      {successLogin ? (
        <Redirect to="/" />
      ) : (
        <>
          <Header />
          <div className="container">
            <div className="h-100 d-flex justify-content-center align-items-center">
              <Box className="form-login">
                <BoxTitle className="text-center text-uppercase">
                  Acesso Restrito
                </BoxTitle>
                <LoginForm submitForm={submitForm} loginData={loginData} />
              </Box>
            </div>
            <div className="d-flex justify-content-center align-items-center">
              {errorLogin.success === false && (
                <Box className="form-login mt-0">
                  <Alert severity="error">
                    <AlertTitle>{errorLogin.error}</AlertTitle>
                  </Alert>
                </Box>
              )}
            </div>
          </div>
        </>
      )}
    </>
  );
};

export default Layout;
