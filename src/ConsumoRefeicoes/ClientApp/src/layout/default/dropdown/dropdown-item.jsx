import React from 'react';
import PropTypes from 'prop-types';
import { Link } from 'react-router-dom';

const DropDownItem = (props) => {
  const { link, descricao } = props;

  return (
    <Link to={link} className="dropdown-item">
      {descricao}
    </Link>
  );
};

DropDownItem.propTypes = {
  descricao: PropTypes.string.isRequired,
  link: PropTypes.string,
};

DropDownItem.defaultProps = {
  link: '#1',
};

export default DropDownItem;
