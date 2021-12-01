import React from 'react';
import Menu from './menu';

import './header.css';

const Header = () => {
  return (
    <nav className="navbar navbar-expand-lg navbar-light bg-verde-escuro">
      <a className="navbar-brand" href="#1">
        <img
          alt="Unimed Chapecó"
          src="https://unimedchapeco.coop.br/assets/img/logo_110_51.png"
        />
      </a>
      <div
        className="collapse navbar-collapse"
        style={{ justifyContent: 'flex-end', color: '#fff' }}>
        <h2>Refeitório</h2>
      </div>

      <div
        className="collapse navbar-collapse"
        id="navbarSupportedContent"
        style={{ justifyContent: 'flex-end' }}>
        <Menu />
      </div>
    </nav>
  );
};

export default Header;
