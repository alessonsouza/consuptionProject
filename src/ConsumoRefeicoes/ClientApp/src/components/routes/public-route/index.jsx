import React from 'react';
import { Route } from 'react-router-dom';

import LayoutLogin from '../../../layout/login';

const DefaultRoute = ({ component: Component, ...props }) => {


  return (
    <Route
      {...props}
      render={(innerProps) =>

          <LayoutLogin>
            <Component {...innerProps} />
          </LayoutLogin>
      }
    />
  );
};

export default DefaultRoute;
