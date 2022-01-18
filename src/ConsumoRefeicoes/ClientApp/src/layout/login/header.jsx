import React, { useState } from 'react';

import './header.css';

const Header = () => {
  const [estaAberto, setEstaAberto] = useState(false);

  const mudaAbertoFechado = () => {
    setEstaAberto(!estaAberto);
  };

  const classNavBarOpen = estaAberto ? 'show' : '';
  const classNavButton = estaAberto ? '' : 'collapsed';

  return (
    <nav className="navbar navbar-expand-lg navbar-light bg-verde-escuro">
      <button
        className={`navbar-toggler ${classNavButton}`}
        type="button"
        data-toggle="collapse"
        data-target="#navbarSupportedContent"
        aria-controls="navbarSupportedContent"
        aria-expanded={estaAberto ? 'true' : 'false'}
        aria-label="Toggle navigation"
        onClick={() => mudaAbertoFechado()}>
        <span className="line" />
        <span className="line" />
        <span className="line" />
      </button>

      <div
        className={`collapse navbar-collapse justify-content-center ${classNavBarOpen}`}
        id="navbarSupportedContent"
      />
    </nav>
  );
};

export default Header;
