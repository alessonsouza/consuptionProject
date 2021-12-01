import React from 'react';
import PropTypes from 'prop-types';
import Header from './header';

const Layout = ({ children }) => {
  return (
    <>
      <Header />
      <main role="main">
        <div className="container-fluid">{children}</div>
      </main>
    </>
  );
};

Layout.propTypes = {
  children: PropTypes.node.isRequired,
};

export default Layout;
