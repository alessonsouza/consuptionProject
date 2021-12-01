import React from 'react';
import PropTypes from 'prop-types';
import { Link, useLocation } from 'react-router-dom';

const MenuItem = (props) => {
  const { label, link, disabled } = props;

  const location = useLocation();

  const classActive = location.pathname === link ? 'active' : '';
  const classDisabled = disabled ? 'disabled' : '';
  const onRemove = () => {
    props.onClick();
  };
  return (
    <li className={`nav-item ${classActive}`}>
      <Link
        to={link}
        className={`nav-link ${classDisabled}`}
        aria-disabled={disabled}
        onClick={onRemove}>
        {label}{' '}
      </Link>
    </li>
  );
};

MenuItem.propTypes = {
  label: PropTypes.string.isRequired,
  link: PropTypes.string.isRequired,
  disabled: PropTypes.bool,
};

MenuItem.defaultProps = {
  disabled: false,
};

export default MenuItem;
