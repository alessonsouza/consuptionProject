import React, { useState } from 'react';
import { Route, Redirect } from 'react-router-dom';
import AuthAPI from '../../../lib/api/auth';
import Layout from '../../../layout/default';

const PrivateRoute = ({ component: Component, ...props }) => {
  const [estaAutenticado, setEstaAutenticado] = useState(async () => {
    await AuthAPI.isAuth().then((res) => {
      setEstaAutenticado(res?.data);
      return res?.data || false;
    });
  });

  return (
    <Route
      {...props}
      render={(innerProps) =>
        estaAutenticado ? (
          <Layout>
            <Component {...innerProps} />
          </Layout>
        ) : (
          <Redirect to="/login" />
        )
      }
    />
  );
};

export default PrivateRoute;
