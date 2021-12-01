import React from 'react';
import PropTypes from 'prop-types';

import './box.css';

const Box = ({ children, className }) => {
  return <div className={`box ${className}`}>{children}</div>;
};

Box.propTypes = {
  children: PropTypes.node.isRequired,
  className: PropTypes.string,
};

Box.defaultProps = {
  className: ' ',
};

export default Box;
