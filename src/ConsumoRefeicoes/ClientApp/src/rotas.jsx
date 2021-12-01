import React, { Suspense } from 'react';
import { Route, Switch } from 'react-router-dom';

import PrivateRoute from './components/routes/private-route';
import DefaultRoute from './components/routes/public-route';
import Loader from './components/loader';
import Home from './pages/default';
import Login from './pages/login';

const Rotas = () => {
  return (
    <Suspense
      fallback={
        <div>
          <Loader loading />
        </div>
      }>
      <Switch>
        <DefaultRoute exact path="/login" component={Login} />
        <PrivateRoute path="/" component={Home} />

        {/*
          <PrivateRoute path="/restrito" component={PaginaRestrita} />
          */}
        <Route path="*" component={() => <h1 classNmae="text-center"> PAGE NOT FOUND </h1>} />
      </Switch>
    </Suspense>
  );
};
export default Rotas;
