import React from 'react';
import PropTypes from 'prop-types';

import './box.css';

const BoxTitle = ({ children, className }) => {
  return <h1 className={`box-title ${className}`}>{children}</h1>;
};

BoxTitle.propTypes = {
  children: PropTypes.node.isRequired,
};

export default BoxTitle;
