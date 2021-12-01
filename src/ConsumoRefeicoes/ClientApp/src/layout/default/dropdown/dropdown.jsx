import React, { useState } from 'react';
import PropTypes from 'prop-types';

const Dropdown = (props) => {
  const { children, descricao } = props;

  const [openMenu, setOpenMenu] = useState(false);

  const abreFechaMenu = () => {
    setOpenMenu(!openMenu);
  };

  const classDropDownOpen = openMenu ? 'show' : '';

  return (
    <>
      <li className={`nav-item dropdown ${classDropDownOpen}`}>
        <a
          className="nav-link dropdown-toggle"
          href="#1"
          id="navbarDropdown"
          role="button"
          data-toggle="dropdown"
          aria-haspopup="true"
          aria-expanded={openMenu}
          onClick={() => abreFechaMenu()}>
          {descricao}
        </a>
        <div
          className={`dropdown-menu ${classDropDownOpen}`}
          aria-labelledby="navbarDropdown">
          {children}
        </div>
      </li>
    </>
  );
};

Dropdown.propTypes = {
  children: PropTypes.node.isRequired,
  descricao: PropTypes.string.isRequired,
};

export default Dropdown;
