import React from 'react';
import PropTypes from 'prop-types';

import './form.css';

const TextArea = ({
  label,
  rows,
  field,
  form: { touched, errors },
  ...props
}) => {
  const campoComErro = touched[field.name] && errors[field.name];
  const classError = campoComErro ? 'form-error' : '';

  return (
    <div className={`form-group ${classError}`}>
      <label htmlFor={field.name} className={campoComErro ? 'form-error' : ''}>
        {label}
      </label>
      <textarea
        className={`form-control ${classError}`}
        id={field.name}
        rows={rows}
        {...field}
        {...props}
      />
      {campoComErro ? (
        <div className="invalid-feedback">{errors[field.name]}</div>
      ) : null}
    </div>
  );
};

TextArea.propTypes = {
  label: PropTypes.string.isRequired,
  rows: PropTypes.number,
};

TextArea.defaultProps = {
  rows: 5,
};

export default TextArea;
